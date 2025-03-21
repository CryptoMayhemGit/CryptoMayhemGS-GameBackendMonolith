using System.Threading.Tasks;

namespace Mayhem.Worker.Notification.Interfaces
{
    /// <summary>
    /// Worker Service
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Resends the notifications asynchronous.
        /// </summary>
        /// <returns></returns>
        Task ResendNotificationsAsync();
    }
}
