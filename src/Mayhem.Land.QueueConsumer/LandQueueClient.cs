using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;

namespace Mayhem.Land.QueueConsumer
{
    public class LandQueueClient : QueueClient, ILandQueueClient
    {
        public LandQueueClient(ServiceBusConnectionStringBuilder connectionStringBuilder, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionStringBuilder, receiveMode, retryPolicy)
        {
        }

        public LandQueueClient(string connectionString, string entityPath, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionString, entityPath, receiveMode, retryPolicy)
        {
        }

        public LandQueueClient(ServiceBusConnection serviceBusConnection, string entityPath, ReceiveMode receiveMode, RetryPolicy retryPolicy) : base(serviceBusConnection, entityPath, receiveMode, retryPolicy)
        {
        }

        public LandQueueClient(string endpoint, string entityPath, ITokenProvider tokenProvider, TransportType transportType = TransportType.Amqp, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(endpoint, entityPath, tokenProvider, transportType, receiveMode, retryPolicy)
        {
        }
    }
}
