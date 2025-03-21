using Mayhem.Consumer.Queue.IntegrationTest.Base;
using Mayhem.Item.QueueConsumer;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest
{
    public class ItemConsumerTests : QueueBaseTest
    {
        private IItemQueueClient consumer;

        [OneTimeSetUp]
        public void Setup()
        {
            publisher = GetItemQueuePublisher();
            consumer = new ItemQueueClient(ItemQueueConnectionString, ItemQueueName);
            consumer.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        [Test]
        public async Task PublishMessageOnQueue_WhenMessagePublished_ThenGetIt_Test() => await PublishAndValidate();
    }
}