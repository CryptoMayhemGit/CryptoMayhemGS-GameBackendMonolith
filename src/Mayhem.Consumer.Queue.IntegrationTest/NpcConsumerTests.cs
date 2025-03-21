using Mayhem.Consumer.Queue.IntegrationTest.Base;
using Mayhem.Npc.QueueConsumer;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest
{
    public class NpcConsumerTests : QueueBaseTest
    {
        private INpcQueueClient consumer;

        [OneTimeSetUp]
        public void Setup()
        {
            publisher = GetNpcQueuePublisher();
            consumer = new NpcQueueClient(NpcQueueConnectionString, NpcQueueName);
            consumer.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        [Test]
        public async Task PublishMessageOnQueue_WhenMessagePublished_ThenGetIt_Test() => await PublishAndValidate();
    }
}