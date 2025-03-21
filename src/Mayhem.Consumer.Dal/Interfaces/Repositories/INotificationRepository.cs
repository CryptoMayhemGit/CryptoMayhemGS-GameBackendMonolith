using Mayhem.Consumer.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Notification Repository
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Sets the notification as sent asynchronous.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns></returns>
        Task<bool> SetNotificationAsSentAsync(long notificationId);
        /// <summary>
        /// Gets the notification by identifier asynchronous.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns></returns>
        Task<NotificationDto> GetNotificationByIdAsync(long notificationId);
    }
}