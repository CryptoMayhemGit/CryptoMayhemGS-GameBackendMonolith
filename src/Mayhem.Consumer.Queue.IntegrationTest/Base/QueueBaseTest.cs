using FluentAssertions;
using Mayhem.Consumer.Queue.IntegrationTest.QueuePublisher.Implementations;
using Mayhem.Consumer.Queue.IntegrationTest.QueuePublisher.Interfaces;
using Mayhem.Messages;
using Mayhem.Queue.Classes;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest.Base
{
    public class QueueBaseTest
    {
        protected const string ItemQueueConnectionString = "Endpoint=sb://mayhem-testconsumer.servicebus.windows.net/;SharedAccessKeyName=policy;SharedAccessKey=lcyd3ckKiQ3M0K2XImqD3UkHJQNz1m9DOZygTa+kkD8=;";
        protected const string LandQueueConnectionString = "Endpoint=sb://mayhem-testconsumer.servicebus.windows.net/;SharedAccessKeyName=policy;SharedAccessKey=zqf4lDGnKo1rbFCnKOj/8Tv5P19ifhKhzeUMahILlCw=;";
        protected const string NotificationQueueConnectionString = "Endpoint=sb://mayhem-testconsumer.servicebus.windows.net/;SharedAccessKeyName=policy;SharedAccessKey=eU7ebbkXP5IR618AOAnyHzDA4I1PqQDY2m2PcGbgyrk=;";
        protected const string NpcQueueConnectionString = "Endpoint=sb://mayhem-testconsumer.servicebus.windows.net/;SharedAccessKeyName=policy;SharedAccessKey=/I2N919JgnVysNQ+ntVhJC9zHHpFo6Wz0lXJh0oH3PE=;";

        protected const string ItemQueueName = "itemqueue";
        protected const string LandQueueName = "landqueue";
        protected const string NotificationQueueName = "notificationqueue";
        protected const string NpcQueueName = "npcqueue";

        protected readonly MessageHandlerOptions messageHandlerOptions = new((ExceptionReceivedEventArgs args) => Task.CompletedTask)
        {
            MaxConcurrentCalls = 1,
            AutoComplete = true
        };
        protected IAzureQueuePublisher publisher;

        private volatile bool haveResponse;
        private TestMessageDto Response;


        protected IAzureQueuePublisher GetItemQueuePublisher()
        {
            return new AzureQueuePublisher(new QueueConfiguration()
            {
                QueueName = ItemQueueName,
                ServiceBusConnectionString = ItemQueueConnectionString,
            });
        }

        protected IAzureQueuePublisher GetLandQueuePublisher()
        {
            return new AzureQueuePublisher(new QueueConfiguration()
            {
                QueueName = LandQueueName,
                ServiceBusConnectionString = LandQueueConnectionString,
            });
        }

        protected IAzureQueuePublisher GetNotificationQueuePublisher()
        {
            return new AzureQueuePublisher(new QueueConfiguration()
            {
                QueueName = NotificationQueueName,
                ServiceBusConnectionString = NotificationQueueConnectionString,
            });
        }

        protected IAzureQueuePublisher GetNpcQueuePublisher()
        {
            return new AzureQueuePublisher(new QueueConfiguration()
            {
                QueueName = NpcQueueName,
                ServiceBusConnectionString = NpcQueueConnectionString,
            });
        }

        protected E FromMessage<E>(Message message)
        {
            if (message.Body == null || message.Body.Length == 1)
            {
                throw ExceptionMessages.EmptyQueueMessageException;
            }

            string body = Encoding.UTF8.GetString(message.Body);

            return JsonConvert.DeserializeObject<E>(body);
        }

        protected async Task ProcessMessagesAsync(Message message, CancellationToken token = default)
        {
            Response = FromMessage<TestMessageDto>(message);
            haveResponse = true;
            await Task.CompletedTask;
        }

        protected async Task PublishAndValidate()
        {
            TestMessageDto message = new()
            {
                Id = new Random().Next(1, 1000),
                Date = DateTime.UtcNow,
                Text = "test message",
            };

            bool result = await publisher.PublishMessage(message);

            while (!haveResponse)
            {
                await Task.Delay(100);
            }

            result.Should().BeTrue();
            Response.Id.Should().Be(message.Id);
            Response.Text.Should().Be(message.Text);
            Response.Date.Should().Be(message.Date);
        }
    }
}
