using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Npc Repository
    /// </summary>
    public interface INpcRepository
    {
        /// <summary>
        /// Gets the hero by nft identifier asynchronous.
        /// </summary>
        /// <param name="heroNftId">The nft identifier.</param>
        /// <returns></returns>
        Task<NpcDto> GetNpcByNftIdAsync(long heroNftId);
        /// <summary>
        /// Gets the user NPC by identifier asynchronous with attributes.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<NpcDto> GetUserNpcByIdWithAttributesAsync(long npcId, int userId);

        /// <summary>
        /// Gets the user NPC by identifier asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<NpcDto> GetUserNpcByIdAsync(long npcId, int userId);
        /// <summary>
        /// Gets the available NPCS by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<NpcDto>> GetAvailableNpcsByUserIdAsync(int userId);
        /// <summary>
        /// Gets the enemy user NPCS asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userLandIds">The user land ids.</param>
        /// <returns></returns>
        Task<IEnumerable<NpcDto>> GetEnemyUserNpcsAsync(int userId, IEnumerable<long> userLandIds);
        /// <summary>
        /// Gets the user NPCS asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<NpcDto>> GetUserNpcsAsync(int userId);

        /// <summary>
        /// Updates the NPC attributes asynchronous.
        /// </summary>
        /// <param name="npcId">The identifier.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="newHealthsState">New state of the healths.</param>
        /// <returns></returns>
        Task<bool> UpdateNpcHealthWithAttributesAsync(long npcId, ICollection<AttributeDto> attributes, NpcHealthsState newHealthsState);
    }
}