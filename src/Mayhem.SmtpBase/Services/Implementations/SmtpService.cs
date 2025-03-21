using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Messages;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Security;
using MailKit.Net.Smtp;
using Mayhem.SmtpBase.Dto;
using Mayhem.SmtpBase.Services.Interfaces;

namespace Mayhem.SmtpBase.Services.Implementations
{
    public class SmtpService : ISmtpService
    {
        private readonly ILogger<SmtpService> logger;
        private readonly NotificationConfigruation notificationConfigruation;

        public SmtpService(IMayhemConfigurationService mayhemConfigurationService, ILogger<SmtpService> logger)
        {
            this.logger = logger;
            notificationConfigruation = mayhemConfigurationService.MayhemConfiguration.NotificationConfigruation;
        }

        public async Task<bool> SendAsync(EmailRequestDto request)
        {
            logger.LogInformation(LoggerMessages.SendingMessageOfSubject(request.Subject, request.To));
            try
            {
                MimeMessage message = new()
                {
                    Subject = request.Subject,
                    Body = new BodyBuilder()
                    {
                        HtmlBody = request.Html,
                    }.ToMessageBody(),
                };

                message.From.Add(new MailboxAddress(notificationConfigruation.SmtpSenderName, request.From ?? notificationConfigruation.SmtpSenderAddress));
                message.To.Add(new MailboxAddress(request.To, request.To));

                return await SendAsync(message);
            }

            catch (Exception e)
            {
                logger.LogDebug(LoggerMessages.ErrorOccurredDuring($"sending email message: {e.Message}"));
                return await Task.FromResult(false);
            }
        }

        private async Task<bool> SendAsync(MimeMessage message)
        {
            SecureSocketOptions secureSocketOptions = notificationConfigruation.StartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None;

            using (SmtpClient smtpClient = new())
            {
                await smtpClient.ConnectAsync(notificationConfigruation.SmtpHostAddress, notificationConfigruation.SmtpHostPort, secureSocketOptions);
                if (!string.IsNullOrEmpty(notificationConfigruation.SmtpHostUser) && !string.IsNullOrEmpty(notificationConfigruation.SmtpHostPass))
                {
                    await smtpClient.AuthenticateAsync(notificationConfigruation.SmtpHostUser, notificationConfigruation.SmtpHostPass);
                }

                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
                return await Task.FromResult(true);
            }
        }
    }
}
