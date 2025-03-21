using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;

namespace Mayhem.Npc.QueueConsumer
{
    public class NpcQueueClient : QueueClient, INpcQueueClient
    {
        public NpcQueueClient(ServiceBusConnectionStringBuilder connectionStringBuilder, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionStringBuilder, receiveMode, retryPolicy)
        {
        }

        public NpcQueueClient(string connectionString, string entityPath, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(connectionString, entityPath, receiveMode, retryPolicy)
        {
        }

        public NpcQueueClient(ServiceBusConnection serviceBusConnection, string entityPath, ReceiveMode receiveMode, RetryPolicy retryPolicy) : base(serviceBusConnection, entityPath, receiveMode, retryPolicy)
        {
        }

        public NpcQueueClient(string endpoint, string entityPath, ITokenProvider tokenProvider, TransportType transportType = TransportType.Amqp, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null) : base(endpoint, entityPath, tokenProvider, transportType, receiveMode, retryPolicy)
        {
        }
    }
}
