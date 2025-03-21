using System.Threading.Tasks;

namespace Mayhem.Explore.Mission.Worker.Interfaces
{
    /// <summary>
    /// Explore Mission Service
    /// </summary>
    public interface IExploreMissionService
    {
        /// <summary>
        /// Starts the work asynchronous.
        /// </summary>
        /// <returns></returns>
        Task StartWorkAsync();
    }
}
