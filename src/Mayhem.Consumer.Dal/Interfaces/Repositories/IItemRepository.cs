using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Item Repository
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// Removes the item from user asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveItemFromUserAsync(long id);
        /// <summary>
        /// Updates the item owner asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="walletAddress">The wallet address.</param>
        /// <returns></returns>
        Task<bool> UpdateItemOwnerAsync(long id, string walletAddress);
    }
}