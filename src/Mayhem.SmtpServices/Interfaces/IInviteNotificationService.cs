using System.Threading.Tasks;
using Mayhem.SmtpServices.Dtos;

namespace Mayhem.SmtpServices.Interfaces
{
    /// <summary>
    /// Invite Notification Service
    /// </summary>
    public interface IInviteNotificationService
    {
        /// <summary>
        /// Sends the notification asynchronous.
        /// </summary>
        /// <param name="emailNotificationDto">The email notification dto.</param>
        /// <returns></returns>
        Task<bool> SendNotificationAsync(EmailNotificationDto emailNotificationDto);
    }
}
