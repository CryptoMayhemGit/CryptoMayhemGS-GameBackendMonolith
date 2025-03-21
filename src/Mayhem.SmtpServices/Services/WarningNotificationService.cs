using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Messages;
using System.Threading.Tasks;
using Mayhem.SmtpServices.Interfaces;
using Mayhem.SmtpBase.Dto;
using Mayhem.SmtpBase.Services.Interfaces;

namespace Mayhem.SmtpServices.Services
{
    public class WarningNotificationService : IWarningNotificationService
    {
        private readonly ISmtpService smtpService;
        private readonly NotificationConfigruation notificationConfigruation;

        public WarningNotificationService(
            ISmtpService smtpService,
            IMayhemConfigurationService mayhemConfigurationService)
        {
            this.smtpService = smtpService;
            notificationConfigruation = mayhemConfigurationService.MayhemConfiguration.NotificationConfigruation;
        }

        public async Task<bool> SendWarningAsync(int id)
        {
            return await smtpService.SendAsync(new EmailRequestDto
            {
                From = notificationConfigruation.SmtpSenderAddress,
                To = notificationConfigruation.AdriaTeamAddress,
                Subject = NotificationMessages.NotificationError,
                Html = NotificationMessages.UnableToSendNotificationWithId(id),
            });
        }
    }
}
