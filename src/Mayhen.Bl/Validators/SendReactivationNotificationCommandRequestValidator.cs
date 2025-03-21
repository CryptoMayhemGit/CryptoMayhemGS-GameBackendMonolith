using FluentValidation;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.SendReativationNotification;
using Mayhen.Bl.Validators.Base;
using Mayhen.Bl.Validators.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mayhen.Bl.Validators
{
    public class SendReactivationNotificationCommandRequestValidator : BaseValidator<SendReactivationNotificationCommandRequest>
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IMayhemDataContext mayhemDataContext;

        public SendReactivationNotificationCommandRequestValidator(
            IMayhemConfigurationService mayhemConfigurationService,
            IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mayhemConfigurationService = mayhemConfigurationService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyNotificationEmail();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(BaseMessages.EmailAddressIsRequiredBaseMessage)
                .MaximumLength(320)
                .EmailAddressValidator().WithMessage(BaseMessages.ValidEmailIsRequiredBaseMessage);
        }

        private void VerifyNotificationEmail()
        {
            RuleFor(x => new { x.Email }).CustomAsync(async (request, context, cancellation) =>
            {
                Notification notification = await mayhemDataContext
                    .Notifications
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == request.Email, cancellation);

                if (notification == null)
                {
                    context.AddFailure(FailureMessages.NotificationWithEmailDoesNotExistFailure(request.Email));
                    return;
                }

                int resendTime = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.ResendNotificationTimeInMinutes;
                if (notification.LastModificationDate.HasValue && notification.LastModificationDate > DateTime.UtcNow.AddMinutes(-resendTime))
                {
                    context.AddFailure(FailureMessages.PleaseWaitBeforeResend(resendTime));
                }
                else if (notification.CreationDate > DateTime.UtcNow.AddMinutes(-resendTime))
                {
                    context.AddFailure(FailureMessages.PleaseWaitBeforeResend(resendTime));
                }
            });
        }
    }
}
