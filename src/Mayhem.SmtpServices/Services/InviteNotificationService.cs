using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Newtonsoft.Json;
using Mayhem.SmtpServices.Dtos;
using Mayhem.SmtpServices.Interfaces;
using Mayhem.SmtpServices.Scripts;
using Mayhem.Messages;
using Mayhem.SmtpBase.Dto;
using Mayhem.SmtpBase.Services.Interfaces;
using Mayhem.Helper;

namespace Mayhem.SmtpServices.Services
{
    public class InviteNotificationService : IInviteNotificationService
    {
        private readonly ISmtpService smtpService;
        private readonly NotificationConfigruation notificationConfigruation;
        private readonly ServiceSecretsConfigruation serviceSecretsConfigruation;

        public InviteNotificationService(
            ISmtpService smtpService,
            IMayhemConfigurationService mayhemConfigurationService)
        {
            this.smtpService = smtpService;
            notificationConfigruation = mayhemConfigurationService.MayhemConfiguration.NotificationConfigruation;
            serviceSecretsConfigruation = mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation;
        }

        public async Task<bool> SendNotificationAsync(EmailNotificationDto emailNotificationDto)
        {
            string activationNotificationLink = GetActivationNotificationLink(emailNotificationDto);

            return await smtpService.SendAsync(new EmailRequestDto
            {
                From = notificationConfigruation.SmtpSenderAddress,
                To = emailNotificationDto.Email,
                Subject = NotificationMessages.AdriaWelcomeMessage,
                Html = GetInviteHtml(activationNotificationLink),
            });

        }

        private string GetInviteHtml(string activationNotificationLink)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scripts", "Invite.html");
            string content = File.ReadAllText(path);

            content = content.Replace(ScriptConst.ActivationLink, activationNotificationLink)
                             .Replace(ScriptConst.ContactLink, $"mailto:{notificationConfigruation.ContactEmail}");

            return content;
        }

        private string GetActivationNotificationToken(EmailNotificationDto emailNotificationDto)
        {
            ActivationNotificationDataDto activationNotificationData = new()
            {
                Wallet = emailNotificationDto.WalletAddress,
                Email = emailNotificationDto.Email,
                CreationDate = DateTime.UtcNow
            };

            string serializeActivationNotificationData = JsonConvert.SerializeObject(activationNotificationData);
            return serializeActivationNotificationData.Encrypt(serviceSecretsConfigruation.ActivationTokenSecretKey);
        }

        private string GetActivationNotificationLink(EmailNotificationDto emailNotificationDto)
        {
            return $"https://crypt0mayhem.io/ultra-aktywacja-konta?token={GetActivationNotificationToken(emailNotificationDto)}";
        }
    }
}
