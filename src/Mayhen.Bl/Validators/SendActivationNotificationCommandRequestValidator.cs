using FluentValidation;
using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Validators.Base;
using Mayhen.Bl.Validators.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class SendActivationNotificationCommandRequestValidator :
        BaseWalletValidator<SendActivationNotificationCommandRequest, SendActivationNotificationCommandResponse>
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IBlockchainService blockchainService;
        private readonly IMayhemDataContext mayhemDataContext;

        public SendActivationNotificationCommandRequestValidator(IMayhemConfigurationService mayhemConfigurationService, IBlockchainService blockchainService, IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.blockchainService = blockchainService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            ValidateWallet(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration, blockchainService);
            VerifyGameUserEmailAndWallet();
            VerifyNotificationEmailAndWallet();
        }

        private void VerifyGameUserEmailAndWallet()
        {
            RuleFor(x => new { x.Wallet, x.Email }).CustomAsync(async (request, context, cancellation) =>
            {
                GameUser existingGameUser = await mayhemDataContext
                    .GameUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.WalletAddress.Equals(request.Wallet)
                    || x.Email == request.Email, cancellation);

                if (existingGameUser?.WalletAddress == request.Wallet)
                {
                    context.AddFailure(FailureMessages.UserWithWalletAlreadyExistsFailure(request.Wallet));
                }
                else if (existingGameUser?.Email == request.Email)
                {
                    context.AddFailure(FailureMessages.UserWithEmailAlreadyExistsFailure(request.Email));
                }
            });
        }

        private void VerifyNotificationEmailAndWallet()
        {
            RuleFor(x => new { x.Wallet, x.Email }).CustomAsync(async (request, context, cancellation) =>
            {
                Notification notification = await mayhemDataContext
                    .Notifications
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.WalletAddress.Equals(request.Wallet)
                    || x.Email == request.Email, cancellation);

                if (notification?.WalletAddress == request.Wallet)
                {
                    context.AddFailure(FailureMessages.NotificationWithWalletAlreadyExistsFailure(request.Wallet));
                }
                else if (notification?.Email == request.Email)
                {
                    context.AddFailure(FailureMessages.NotificationWithEmailAlreadyExistsFailure(request.Email));
                }
            });
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(BaseMessages.EmailAddressIsRequiredBaseMessage)
                .MaximumLength(320)
                .EmailAddressValidator().WithMessage(BaseMessages.ValidEmailIsRequiredBaseMessage);
        }
    }
}
