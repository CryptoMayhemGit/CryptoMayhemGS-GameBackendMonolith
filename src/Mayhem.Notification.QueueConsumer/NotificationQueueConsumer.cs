using Mayhem.Consumer.Dal.Dto.Dtos;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhem.Queue.Consumer.Base.Services;
using Mayhem.SmtpServices.Dtos;
using Mayhem.SmtpServices.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Notification.QueueConsumer
{
    public class NotificationQueueConsumer : AzureQueueConsumer<NotificationQueueConsumer>
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IInviteNotificationService notificationService;

        public NotificationQueueConsumer(ILogger<NotificationQueueConsumer> logger,
                                INotificationQueueClient npcQueueClient,
                                INotificationRepository notificationRepository,
                                IInviteNotificationService notificationService)
            : base(logger, npcQueueClient)
        {
            this.notificationRepository = notificationRepository;
            this.notificationService = notificationService;
        }

        public override async Task ProcessMessagesAsync(Message message, CancellationToken token = default)
        {
            string encodedNotificationId = Encoding.UTF8.GetString(message.Body);

            bool isNumeric = long.TryParse(encodedNotificationId, out long notificationId);

            if (isNumeric == false)
            {
                logger.LogDebug(LoggerMessages.NotificationQueueValueIsNotNumeric(notificationId));
            }
            else
            {
                NotificationDto notificationDto = await notificationRepository.GetNotificationByIdAsync(notificationId);

                bool isSuccess = notificationDto != null;
                if (isSuccess == true)
                {
                    isSuccess = await notificationService.SendNotificationAsync(new EmailNotificationDto()
                    {
                        Id = notificationDto.Id,
                        Email = notificationDto.Email,
                        WalletAddress = notificationDto.WalletAddress,  
                    });
                }

                if (isSuccess == true)
                {
                    isSuccess = await notificationRepository.SetNotificationAsSentAsync(notificationId);
                }

                if (isSuccess == true)
                {
                    logger.LogDebug(LoggerMessages.NotificationWithIdHasBeenSent(encodedNotificationId));
                }
                else
                {
                    logger.LogWarning(LoggerMessages.UnableToSendNotificationWithId(encodedNotificationId));
                }
            }
        }
    }
}
