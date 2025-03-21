using FluentValidation;
using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Configuration.Services;
using Mayhem.Helper;
using Mayhem.Messages;
using Mayhen.Bl.Commands.Base;
using System;

namespace Mayhen.Bl.Validators.Base
{
    public class BaseWalletValidator<T, R> : BaseValidator<T>
        where T : WalletCommandRequest<R>
    {
        public void ValidateWallet(CommonConfiguration commonConfiguration, IBlockchainService blockchainService)
        {
            if (!commonConfiguration.BlockchainAuthorizationEnabled)
            {
                return;
            }

            RuleFor(x => x.Wallet).NotEmpty().WithMessage(BaseMessages.WalletIsRequiredBaseMessage);
            RuleFor(x => x.Wallet).NotEmpty().MaximumLength(200);
            RuleFor(x => x.SignedMessage).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.MessageToSign.Message).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.MessageToSign.Nonce).GreaterThan(0);
            RuleFor(x => x.MessageToSign.Nonce.FromUnixTime()).LessThan(DateTime.UtcNow.AddSeconds(1));
            RuleFor(x => x.MessageToSign.Nonce.FromUnixTime()
                .AddMinutes(commonConfiguration.NonceLifetimeInMinutes))
                .GreaterThan(DateTime.UtcNow);

            VerifyWalletWithSignedMessage(blockchainService);
        }

        private void VerifyWalletWithSignedMessage(IBlockchainService blockchainService)
        {
            RuleFor(x => new { x.MessageToSign, x.SignedMessage, x.Wallet }).MustAsync(async (request, cancellation) =>
            {
                return await blockchainService.VerifyWalletWithSignedMessageAsync(request.Wallet, $"{request.MessageToSign.Message} {request.MessageToSign.Nonce}", request.SignedMessage);
            }).WithMessage(x => BaseMessages.WalletSignatureValidationWasUnsuccessfulForWalletBaseMessage(x.Wallet));
        }
    }
}
