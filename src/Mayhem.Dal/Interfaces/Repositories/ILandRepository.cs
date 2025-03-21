using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Land Repository
    /// </summary>
    public interface ILandRepository
    {
        /// <summary>
        /// Adds the lands asynchronous.
        /// </summary>
        /// <param name="lands">The lands.</param>
        /// <returns></returns>
        Task<IEnumerable<LandDto>> AddLandsAsync(IEnumerable<LandDto> lands);
        /// <summary>
        /// Gets the land by NFT identifier asynchronous.
        /// </summary>
        /// <param name="landNftId">The land NFT identifier.</param>
        /// <returns></returns>
        Task<LandDto> GetLandByNftIdAsync(long landNftId);
        /// <summary>
        /// Gets the simple lands by instance identifier asynchronous.
        /// </summary>
        /// <param name="instanceId">The instance identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<SimpleLandDto>> GetSimpleLandsByInstanceIdAsync(int instanceId);
    }
}
