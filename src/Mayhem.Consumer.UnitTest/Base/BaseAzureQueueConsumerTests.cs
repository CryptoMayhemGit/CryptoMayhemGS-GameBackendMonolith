using Mayhem.Queue.Consumer.Base.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Consumer.UnitTest.Base
{
    public class BaseAzureQueueConsumerTests<T> : AzureQueueConsumer<T>
    {
        public BaseAzureQueueConsumerTests(ILogger<AzureQueueConsumer<T>> logger, IQueueClient queueClient) : base(logger, queueClient)
        {
        }

        public E FromMessageTest<E>(Message message)
        {
            return FromMessage<E>(message);
        }

        public override Task ProcessMessagesAsync(Message message, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}