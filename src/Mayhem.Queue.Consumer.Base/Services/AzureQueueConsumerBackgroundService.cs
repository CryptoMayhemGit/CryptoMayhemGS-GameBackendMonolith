using Mayhem.Queue.Consumer.Base.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Queue.Consumer.Base.Services
{
    public class AzureQueueConsumerBackgroundService<T> : BackgroundService
    {
        private readonly IQueueConsumer<T> _azureQueueConsumer;

        public AzureQueueConsumerBackgroundService(IQueueConsumer<T> azureQueueConsumer)
        {
            _azureQueueConsumer = azureQueueConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _azureQueueConsumer.RegisterOnMessageHandler();
            return Task.CompletedTask;
        }
    }
}
