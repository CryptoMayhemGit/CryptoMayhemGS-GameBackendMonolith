using FluentValidation;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Helper;
using Mayhem.Messages;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Mayhen.Bl.Validators
{
    public class RegisterCommandRequestValidator : BaseValidator<RegisterCommandRequest>
    {
        private readonly string activationKey;
        private readonly IMayhemDataContext mayhemDataContext;

        public RegisterCommandRequestValidator(IMayhemConfigurationService mayhemConfigurationService, IMayhemDataContext mayhemDataContext)
        {
            activationKey = mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.ActivationTokenSecretKey;
            this.mayhemDataContext = mayhemDataContext;

            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyNotyficationStructure();
            VerifyWallet();
            VerifyNotification();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.ActivationNotificationToken).NotEmpty().MaximumLength(2000);
        }

        private void VerifyNotyficationStructure()
        {
            RuleFor(x => new { x.ActivationNotificationToken }).Custom((request, context) =>
            {
                bool result = request.ActivationNotificationToken.TryDecrypt(activationKey);
                if (!result)
                {
                    context.AddFailure(FailureMessages.ActivationTokenIsInvalidOrHasExpiredFailure());
                }

                return;
            });
        }

        private void VerifyWallet()
        {
            RuleFor(x => new { x.ActivationNotificationToken }).CustomAsync(async (request, context, cancellation) =>
            {
                if (!request.ActivationNotificationToken.TryDecrypt(activationKey))
                {
                    return;
                }
                string serializeActivationNotificationData = request.ActivationNotificationToken.Decrypt(activationKey);

                NotificationDataDto notificationData = JsonConvert.DeserializeObject<NotificationDataDto>(serializeActivationNotificationData);

                GameUser existingGameUser = await mayhemDataContext
                    .GameUsers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.WalletAddress.Equals(notificationData.Wallet), cancellation);

                if (existingGameUser != null)
                {
                    context.AddFailure(FailureMessages.UserWithWalletAlreadyExistsFailure(notificationData.Wallet));
                }
            });
        }

        private void VerifyNotification()
        {
            RuleFor(x => new { x.ActivationNotificationToken }).CustomAsync(async (request, context, cancellation) =>
            {
                if (!request.ActivationNotificationToken.TryDecrypt(activationKey))
                {
                    return;
                }
                string serializeActivationNotificationData = request.ActivationNotificationToken.Decrypt(activationKey);

                NotificationDataDto notificationData = JsonConvert.DeserializeObject<NotificationDataDto>(serializeActivationNotificationData);

                Notification notification = await mayhemDataContext
                   .Notifications
                   .SingleOrDefaultAsync(x => x.WalletAddress.Equals(notificationData.Wallet)
                   && x.Email == notificationData.Email, cancellation);

                if (notification == null)
                {
                    context.AddFailure(FailureMessages.ActivationTokenIsInvalidOrHasExpiredFailure());
                    return;
                }
                if (notification.WasSent == false)
                {
                    context.AddFailure(FailureMessages.ActivationTokenIsInvalidOrHasExpiredFailure());
                }
            });
        }
    }
}
