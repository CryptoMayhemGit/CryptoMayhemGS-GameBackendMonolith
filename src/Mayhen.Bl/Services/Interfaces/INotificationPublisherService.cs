using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Notification Publisher Service
    /// </summary>
    public interface INotificationPublisherService
    {
        /// <summary>
        /// Publishes the message.
        /// </summary>
        /// <param name="notificationId">The notification Id.</param>
        /// <returns></returns>
        Task PublishMessageAsync(int notificationId);
    }
}