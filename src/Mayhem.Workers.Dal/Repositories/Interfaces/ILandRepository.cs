using Mayhem.Worker.Dal.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Land Repository
    /// </summary>
    public interface ILandRepository
    {
        /// <summary>
        /// Gets the land NPCS asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<UserLandNpcDto>> GetLandNpcsAsync(long landId);
        /// <summary>
        /// Adds the fog to lands asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        Task AddFogToLandsAsync(long landId, int userId, long npcId);
        /// <summary>
        /// Removes the fog from lands asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task RemoveFogFromLandsAsync(long landId, int userId);
    }
}
