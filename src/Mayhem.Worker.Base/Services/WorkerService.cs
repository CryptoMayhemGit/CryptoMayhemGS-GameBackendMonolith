using Mayhem.Blockchain.Enums;
using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Blockchain.Responses;
using Mayhem.Configuration.Interfaces;
using Mayhem.Messages;
using Mayhem.Worker.Base.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Queue.Dto;
using Mayhem.Queue.Publisher.Base.Interfaces;

namespace Mayhem.Worker.Base.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<WorkerService> logger;
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IBlockchainService blockchainService;
        private readonly IBlockRepository blockRepository;

        public WorkerService(ILogger<WorkerService> logger,
                             IMayhemConfigurationService mayhemConfigurationService,
                             IBlockchainService blockchainService,
                             IBlockRepository blockRepository)
        {
            this.logger = logger;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.blockchainService = blockchainService;
            this.blockRepository = blockRepository;
        }

        public async Task UpdateAsync(IQueueService queueService, BlocksType blockType)
        {
            long fromBlock = (await blockRepository.GetLastBlockAsync(blockType)).LastBlock;
            long toBlock = await blockchainService.GetCurrentBlockAsync();

            if (fromBlock == toBlock)
            {
                return;
            }
            toBlock = ValidateAndChangeBlock(fromBlock, toBlock);
            List<GetLogResult> response = await blockchainService.GetTokenLogsAsync(blockType, fromBlock, toBlock);

            if (response.Count == 0)
            {
                return;
            }

            TransferQueueMessage message = new()
            {
                Dtos = new List<TransferQueueDto>()
            };

            foreach (GetLogResult log in response)
            {
                message.Dtos.Add(new TransferQueueDto()
                {
                    From = log.Topics[0],
                    To = log.Topics[1],
                    Value = log.Topics[2],
                });
            }

            bool status = await PublishMessageAsync(message, queueService);
            if (status)
            {
                await blockRepository.UpdateLastBlockAsync(toBlock, blockType);
            }
            logger.LogDebug(LoggerMessages.PublishMessageWith(message.Dtos.Count, status));
        }

        public async Task<bool> PublishMessageAsync(TransferQueueMessage message, IQueueService queueService)
        {
            try
            {
                return await queueService.PublishMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(PublishMessageAsync)));
                return false;
            }
        }

        public long ValidateAndChangeBlock(long fromBlock, long toBlock)
        {
            if (toBlock - fromBlock >= mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.MaxBlocksToProcessed)
            {
                return fromBlock + mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.MaxBlocksToProcessed;
            }

            return toBlock;
        }
    }
}
