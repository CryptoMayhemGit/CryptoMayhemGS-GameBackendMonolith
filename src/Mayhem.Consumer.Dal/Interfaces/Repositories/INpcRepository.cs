using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Npc Repository
    /// </summary>
    public interface INpcRepository
    {
        /// <summary>
        /// Removes the NPC from user asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveNpcFromUserAsync(long id);
        /// <summary>
        /// Updates the NPC owner asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="walletAddress">The wallet address.</param>
        /// <returns></returns>
        Task<bool> UpdateNpcOwnerAsync(long id, string walletAddress);
    }
}