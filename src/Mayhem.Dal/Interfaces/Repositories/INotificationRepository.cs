using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Notification Repository
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Send activation notification the asynchronous.
        /// </summary>
        /// <param name="sendActivationNotificationCommandRequest">The required data to add activation notification to queue.</param>
        /// <returns></returns>
        Task<int> AddNotificationAsync(SendActivationNotificationCommandRequestDto sendActivationNotificationCommandRequest);
        /// <summary>
        /// Gets the notification by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<NotificationDto> GetNotificationByEmailAsync(string email);

        /// <summary>
        /// Checks the activation link asynchronous.
        /// </summary>
        /// <param name="wallet">The wallet.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> CheckActivationLinkAsync(string wallet, string email);
        /// <summary>
        /// Updates the notification date.
        /// </summary>
        /// <param name="notificationId">The identifier.</param>
        /// <returns></returns>
        Task UpdateNotificationDate(int notificationId);
    }
}