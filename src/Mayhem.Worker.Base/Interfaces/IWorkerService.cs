using Mayhem.Blockchain.Enums;
using Mayhem.Queue.Dto;
using Mayhem.Queue.Publisher.Base.Interfaces;
using System.Threading.Tasks;

namespace Mayhem.Worker.Base.Interfaces
{
    /// <summary>
    /// Worker Service
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="queueService">The queue service.</param>
        /// <param name="blockType">Type of the block.</param>
        /// <returns></returns>
        Task UpdateAsync(IQueueService queueService, BlocksType blockType);
        /// <summary>
        /// Publishes the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="queueService">The queue service.</param>
        /// <returns></returns>
        Task<bool> PublishMessageAsync(TransferQueueMessage message, IQueueService queueService);
        /// <summary>
        /// Validates the and change block.
        /// </summary>
        /// <param name="fromBlock">From block.</param>
        /// <param name="toBlock">To block.</param>
        /// <returns></returns>
        long ValidateAndChangeBlock(long fromBlock, long toBlock);
    }
}