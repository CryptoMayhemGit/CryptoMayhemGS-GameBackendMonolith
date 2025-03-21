using Mayhem.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Discovery Mission Repository
    /// </summary>
    public interface IDiscoveryMissionRepository
    {
        /// <summary>
        /// Discovers the mission asynchronous.
        /// </summary>
        /// <param name="discoveryMissionDto">The discovery mission dto.</param>
        /// <returns></returns>
        Task<DiscoveryMissionDto> DiscoverMissionAsync(DiscoveryMissionDto discoveryMissionDto);
    }
}
