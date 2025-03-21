using Mayhem.Worker.Dal.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Notification Repository
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Gets the old not sent notyfications asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NotificationDto>> GetOldNotSentNotyficationsAsync();
        /// <summary>
        /// Updates the notification attempt asynchronous.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns></returns>
        Task UpdateNotificationAttemptAsync(int notificationId);
    }
}
