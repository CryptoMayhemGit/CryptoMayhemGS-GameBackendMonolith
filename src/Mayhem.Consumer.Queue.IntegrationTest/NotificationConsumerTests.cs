using Mayhem.Consumer.Queue.IntegrationTest.Base;
using Mayhem.Notification.QueueConsumer;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest
{
    public class NotificationConsumerTests : QueueBaseTest
    {
        private INotificationQueueClient consumer;

        [OneTimeSetUp]
        public void Setup()
        {
            publisher = GetNotificationQueuePublisher();
            consumer = new NotificationQueueClient(NotificationQueueConnectionString, NotificationQueueName);
            consumer.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        [Test]
        public async Task PublishMessageOnQueue_WhenMessagePublished_ThenGetIt_Test() => await PublishAndValidate();
    }
}
