using Mayhem.Worker.Dal.Dto;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Interface
{
    /// <summary>
    /// Block Repository
    /// </summary>
    public interface IBlockRepository
    {
        /// <summary>
        /// Gets the last block asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<BlockDto> GetLastBlockAsync();
        /// <summary>
        /// Updates the last block asynchronous.
        /// </summary>
        /// <param name="newBlockValue">The new block value.</param>
        /// <returns></returns>
        Task UpdateLastBlockAsync(long newBlockValue);
    }
}
