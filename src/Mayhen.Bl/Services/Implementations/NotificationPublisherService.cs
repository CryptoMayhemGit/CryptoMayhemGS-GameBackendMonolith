using Mayhem.Messages;
using Mayhem.Queue.Publisher;
using Mayhen.Bl.Services.Interfaces;
using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Implementations
{
    public class NotificationPublisherService : INotificationPublisherService
    {
        private readonly INotificationQueuePublisher notificationQueuePublisher;

        public NotificationPublisherService(INotificationQueuePublisher notificationQueuePublisher)
        {
            this.notificationQueuePublisher = notificationQueuePublisher;
        }

        public async Task PublishMessageAsync(int notificationId)
        {
            try
            {
                await notificationQueuePublisher.PublishMessage(notificationId);
            }
            catch (System.Exception ex)
            {
                throw ExceptionMessages.PublishException(ex);
            }
        }
    }
}
