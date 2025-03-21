using System.Threading.Tasks;

namespace Mayhem.SmtpServices.Interfaces
{
    /// <summary>
    /// Warning Notification Service
    /// </summary>
    public interface IWarningNotificationService
    {
        /// <summary>
        /// Sends the warning asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> SendWarningAsync(int id);
    }
}
