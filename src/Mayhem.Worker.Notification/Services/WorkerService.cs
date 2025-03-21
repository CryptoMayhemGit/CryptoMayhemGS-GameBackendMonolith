using Mayhem.Configuration.Interfaces;
using Mayhem.Messages;
using Mayhem.Queue.Publisher;
using Mayhem.SmtpServices.Interfaces;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Notification.Interfaces;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Worker.Notification.Services
{
    public class WorkerService : IWorkerService
    {
        public readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly INotificationRepository notificationRepository;
        private readonly ILogger<WorkerService> logger;
        private readonly INotificationQueuePublisher notificationQueuePublisher;
        private readonly IWarningNotificationService warningNotificationService;

        public WorkerService(
            INotificationRepository notificationRepository,
            ILogger<WorkerService> logger,
            INotificationQueuePublisher notificationQueuePublisher,
            IMayhemConfigurationService mayhemConfigurationService,
            IWarningNotificationService warningNotificationService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.notificationQueuePublisher = notificationQueuePublisher;
            this.warningNotificationService = warningNotificationService;
        }

        public async Task ResendNotificationsAsync()
        {
            IEnumerable<NotificationDto> notificationIds = await notificationRepository.GetOldNotSentNotyficationsAsync();

            foreach (NotificationDto notification in notificationIds)
            {
                if (notification.Attempts == mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.MaxNotificationAttempts)
                {
                    await warningNotificationService.SendWarningAsync(notification.Id);
                    await notificationRepository.UpdateNotificationAttemptAsync(notification.Id);
                    logger.LogInformation(LoggerMessages.NotificationHasBeenSentToTeam);
                }
                else if (notification.Attempts < mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.MaxNotificationAttempts)
                {
                    await notificationQueuePublisher.PublishMessage(notification.Id);
                    await notificationRepository.UpdateNotificationAttemptAsync(notification.Id);
                    logger.LogInformation(LoggerMessages.PublishMessageToQueueNotification); 
                }
            }
        }
    }
}
