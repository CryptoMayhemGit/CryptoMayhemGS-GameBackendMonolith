using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Queue.Consumer.Base.Interfaces
{
    /// <summary>
    /// Interface for queue message consumers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueConsumer<T>
    {
        /// <summary>
        /// Register a message handler.
        /// </summary>
        void RegisterOnMessageHandler();
        /// <summary>
        /// Override method to implement message proccessing.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task ProcessMessagesAsync(Message message, CancellationToken token = default);
    }
}
