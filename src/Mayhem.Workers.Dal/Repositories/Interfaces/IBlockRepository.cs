using Mayhem.Blockchain.Enums;
using Mayhem.Worker.Dal.Dto;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Block Repository
    /// </summary>
    public interface IBlockRepository
    {
        /// <summary>
        /// Gets the last block asynchronous.
        /// </summary>
        /// <param name="blockType">Type of the block.</param>
        /// <returns></returns>
        Task<BlockDto> GetLastBlockAsync(BlocksType blockType);
        /// <summary>
        /// Updates the last block asynchronous.
        /// </summary>
        /// <param name="newBlockValue">The new block value.</param>
        /// <param name="blockType">Type of the block.</param>
        /// <returns></returns>
        Task UpdateLastBlockAsync(long newBlockValue, BlocksType blockType);
    }
}
