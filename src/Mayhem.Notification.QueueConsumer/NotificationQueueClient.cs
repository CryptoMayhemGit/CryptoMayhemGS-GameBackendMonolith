using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;

namespace Mayhem.Notification.QueueConsumer
{
    public class NotificationQueueClient : QueueClient, INotificationQueueClient
    {
        public NotificationQueueClient(ServiceBusConnectionStringBuilder connectionStringBuilder, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionStringBuilder, receiveMode, retryPolicy)
        {
        }

        public NotificationQueueClient(string connectionString, string entityPath, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionString, entityPath, receiveMode, retryPolicy)
        {
        }

        public NotificationQueueClient(ServiceBusConnection serviceBusConnection, string entityPath, ReceiveMode receiveMode, RetryPolicy retryPolicy) : base(serviceBusConnection, entityPath, receiveMode, retryPolicy)
        {
        }

        public NotificationQueueClient(string endpoint, string entityPath, ITokenProvider tokenProvider, TransportType transportType = TransportType.Amqp, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(endpoint, entityPath, tokenProvider, transportType, receiveMode, retryPolicy)
        {
        }
    }
}
