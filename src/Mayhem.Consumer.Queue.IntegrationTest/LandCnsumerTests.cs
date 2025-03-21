using Mayhem.Consumer.Queue.IntegrationTest.Base;
using Mayhem.Land.QueueConsumer;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest
{
    public class LandCnsumerTests : QueueBaseTest
    {
        private ILandQueueClient consumer;

        [OneTimeSetUp]
        public void Setup()
        {
            publisher = GetLandQueuePublisher();
            consumer = new LandQueueClient(LandQueueConnectionString, LandQueueName);
            consumer.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        [Test]
        public async Task PublishMessageOnQueue_WhenMessagePublished_ThenGetIt_Test() => await PublishAndValidate();
    }
}
