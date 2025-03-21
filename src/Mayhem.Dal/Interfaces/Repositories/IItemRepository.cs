using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Item Repository
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// Gets the item by nft identifier asynchronous.
        /// </summary>
        /// <param name="itemNftId">The nft identifier.</param>
        /// <returns></returns>
        Task<ItemDto> GetItemByNftIdAsync(long itemNftId);
        /// <summary>
        /// Gets the available items by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ItemDto>> GetAvailableItemsByUserIdAsync(int userId);
        /// <summary>
        /// Gets the unavailable items by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ItemDto>> GetUnavailableItemsByUserIdAsync(int userId);
        /// <summary>
        /// Assigns the item to NPC asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> AssignItemToNpcAsync(long npcId, long itemId, int userId);
        /// <summary>
        /// Releases the item from NPC asynchronous.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> ReleaseItemFromNpcAsync(long itemId, int userId);
    }
}