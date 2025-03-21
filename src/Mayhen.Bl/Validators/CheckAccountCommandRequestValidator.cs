using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.CheckAccount;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class CheckAccountCommandRequestValidator : BaseValidator<CheckAccountCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public CheckAccountCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyAccountExistence();
            VerifyNotificationsExistence();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.WalletAddress).NotEmpty().WithMessage(BaseMessages.WalletIsRequiredBaseMessage);
            RuleFor(x => x.WalletAddress).NotEmpty().MaximumLength(200);
        }

        private void VerifyAccountExistence()
        {
            RuleFor(x => x.WalletAddress).CustomAsync(async (wallet, context, cancellation) =>
           {
               GameUser user = await mayhemDataContext
                   .GameUsers
                   .SingleOrDefaultAsync(x => x.WalletAddress == wallet, cancellationToken: cancellation);

               if (user != null)
               {
                   context.AddFailure(FailureMessages.AccountWithWalletAlreadyExistsFailure(wallet));
               }
           });
        }

        private void VerifyNotificationsExistence()
        {
            RuleFor(x => x.WalletAddress).CustomAsync(async (wallet, context, cancellation) =>
            {
                Notification notification = await mayhemDataContext
                    .Notifications
                    .SingleOrDefaultAsync(x => x.WalletAddress == wallet, cancellationToken: cancellation);

                if (notification != null)
                {
                    context.AddFailure(FailureMessages.NotificationWithWalletAlreadyExistsFailure(wallet));
                }
            });
        }
    }
}
