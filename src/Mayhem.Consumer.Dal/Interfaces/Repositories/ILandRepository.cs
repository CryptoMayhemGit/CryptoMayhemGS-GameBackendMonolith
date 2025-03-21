using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Land Repository
    /// </summary>
    public interface ILandRepository
    {
        /// <summary>
        /// Removes the land from user asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveLandFromUserAsync(long id);
        /// <summary>
        /// Updates the land owner asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="walletAddress">The wallet address.</param>
        /// <returns></returns>
        Task<bool> UpdateLandOwnerAsync(long id, string walletAddress);
    }
}