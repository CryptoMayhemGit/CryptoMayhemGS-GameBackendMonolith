using System.Threading.Tasks;

namespace Mayhem.Discovery.Mission.Worker.Interfaces
{
    /// <summary>
    /// Discovery Mission Service
    /// </summary>
    public interface IDiscoveryMissionService
    {
        /// <summary>
        /// Starts the work asynchronous.
        /// </summary>
        /// <returns></returns>
        Task StartWorkAsync();
    }
}
