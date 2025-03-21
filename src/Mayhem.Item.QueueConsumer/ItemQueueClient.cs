using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;

namespace Mayhem.Item.QueueConsumer
{
    public class ItemQueueClient : QueueClient, IItemQueueClient
    {
        public ItemQueueClient(ServiceBusConnectionStringBuilder connectionStringBuilder, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionStringBuilder, receiveMode, retryPolicy)
        {
        }

        public ItemQueueClient(string connectionString, string entityPath, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionString, entityPath, receiveMode, retryPolicy)
        {
        }

        public ItemQueueClient(ServiceBusConnection serviceBusConnection, string entityPath, ReceiveMode receiveMode, RetryPolicy retryPolicy) : base(serviceBusConnection, entityPath, receiveMode, retryPolicy)
        {
        }

        public ItemQueueClient(string endpoint, string entityPath, ITokenProvider tokenProvider, TransportType transportType = TransportType.Amqp, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(endpoint, entityPath, tokenProvider, transportType, receiveMode, retryPolicy)
        {
        }
    }
}
