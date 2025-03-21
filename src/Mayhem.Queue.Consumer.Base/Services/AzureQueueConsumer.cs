using Mayhem.Messages;
using Mayhem.Queue.Consumer.Base.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Queue.Consumer.Base.Services
{
    public abstract class AzureQueueConsumer<T> : IQueueConsumer<T>
    {
        protected readonly ILogger<AzureQueueConsumer<T>> logger;
        protected readonly IQueueClient queueClient;

        protected const int MaxConcurrentCalls = 1;
        protected const bool AutoComplete = true;

        public AzureQueueConsumer(ILogger<AzureQueueConsumer<T>> logger, IQueueClient queueClient)
        {
            this.logger = logger;
            this.queueClient = queueClient;
        }

        public abstract Task ProcessMessagesAsync(Message message, CancellationToken token = default);

        public virtual void RegisterOnMessageHandler()
        {
            logger.LogDebug(LoggerMessages.RegisteringMessageHandler);

            MessageHandlerOptions messageHandlerOptions = new(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = MaxConcurrentCalls,
                AutoComplete = AutoComplete
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        protected E FromMessage<E>(Message message)
        {
            if (message.Body == null || message.Body.Length == 1)
            {
                logger.LogError(LoggerMessages.MessageHasNullOrEmptyBody);
                throw ExceptionMessages.EmptyQueueMessageException;
            }

            logger.LogDebug(LoggerMessages.GettingBodyForMessage);

            string body = Encoding.UTF8.GetString(message.Body);
            logger.LogDebug(LoggerMessages.MessageBody(body));

            return JsonConvert.DeserializeObject<E>(body);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            logger.LogError(LoggerMessages.MessageHandlerEncounteredException(exceptionReceivedEventArgs.Exception));

            ExceptionReceivedContext context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            logger.LogError(LoggerMessages.ExceptionContextForTroubleshooting(context.Endpoint, context.EntityPath, context.Action));

            return Task.CompletedTask;
        }
    }
}
