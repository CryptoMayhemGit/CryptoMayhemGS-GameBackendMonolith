using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Npc Repository
    /// </summary>
    public interface INpcRepository
    {
        /// <summary>
        /// Gets the NPC asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        Task<NpcDto> GetNpcAsync(long npcId);
        /// <summary>
        /// Updates the NPC land asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="landToId">The land to identifier.</param>
        /// <returns></returns>
        Task UpdateNpcLandAsync(long id, long landToId);
        /// <summary>
        /// Updates the NPC status asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="npcStatusId">The NPC status identifier.</param>
        /// <returns></returns>
        Task UpdateNpcStatusAsync(long id, NpcsStatus npcStatusId);
    }
}
