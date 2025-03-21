using Mayhem.Consumer.Queue.IntegrationTest.QueuePublisher.Interfaces;
using Mayhem.Queue.Interfaces;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest.QueuePublisher.Implementations
{
    public class AzureQueuePublisher : IAzureQueuePublisher
    {
        private readonly IQueueClient queueClient;

        public AzureQueuePublisher(IQueueConfiguration queueConfiguration)
        {
            queueClient = new QueueClient(queueConfiguration.ServiceBusConnectionString, queueConfiguration.QueueName);
        }

        public async Task<bool> PublishMessage(object message)
        {
            if (message == null)
            {
                return false;
            }

            Message messageBody = ToMessage(JsonConvert.SerializeObject(message));

            await queueClient.SendAsync(messageBody);

            return true;
        }

        private static Message ToMessage(string json)
        {
            byte[] body = Encoding.UTF8.GetBytes(json);

            Message message = new()
            {
                Body = body,
                ContentType = "text/plain"
            };

            return message;
        }
    }
}
