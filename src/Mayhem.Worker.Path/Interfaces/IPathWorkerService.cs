using System.Threading.Tasks;

namespace Mayhem.Worker.Path.Interfaces
{
    /// <summary>
    /// Path Worker Service
    /// </summary>
    public interface IPathWorkerService
    {
        /// <summary>
        /// Starts the work asynchronous.
        /// </summary>
        /// <returns></returns>
        Task StartWorkAsync();
    }
}
