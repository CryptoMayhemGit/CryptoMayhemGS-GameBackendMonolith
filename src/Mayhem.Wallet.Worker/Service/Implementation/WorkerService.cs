using Mayhem.Wallet.Worker.Configuration;
using Mayhem.Wallet.Worker.Models;
using Mayhem.Wallet.Worker.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Implementation
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<WorkerService> logger;
        private readonly IBlockchainService blockchainService;
        private readonly IBlockRepository blockRepository;
        private readonly IGameUserRepository gameUserRepository;

        public WorkerService(ILogger<WorkerService> logger,
                             IBlockchainService blockchainService,
                             IBlockRepository blockRepository,
                             IGameUserRepository gameUserRepository)
        {
            this.logger = logger;
            this.blockchainService = blockchainService;
            this.blockRepository = blockRepository;
            this.gameUserRepository = gameUserRepository;
        }

        public async Task UpdateAsync()
        {
            try
            {
                long lastIndex = (await blockRepository.GetLastBlockAsync()).LastBlock;
                long initialValue = lastIndex;

                int amount = await blockchainService.GetVestingSchedulesCountFunctionAsync();
                for (long index = (int)lastIndex; index < amount; index++)
                {
                    lastIndex = index;
                    byte[] details = await blockchainService.GetVestingIdAtIndexFunctionAsync(index);
                    VestingSchedule vestingDetail = await blockchainService.GetVestingScheduleFunctionAsync(details);
                    string address = vestingDetail.beneficiary;
                    int usdcAmount = (int)(BigInteger.Divide(BigInteger.Divide(vestingDetail.amountTotal, BigInteger.Parse(MayhemConfiguration.Divider)), BigInteger.Parse(MayhemConfiguration.AdriaTokenPerOneUsdc)));
                    int voteCategoryId = await gameUserRepository.GetVoteCategoryIdByInvestorCategory(InvestorCategory.ThirdSale);

                    logger.LogInformation($"Index {index} - portfel ({address}) został dodany do early access oraz zakupił {usdcAmount} usdc.");
                    await gameUserRepository.AddWalletAsync(address, voteCategoryId, usdcAmount);
                }

                if (initialValue != amount)
                {
                    await blockRepository.UpdateLastBlockAsync(amount);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"ErrorMessage: {ex.Message} StackTrace: {ex.StackTrace}");
            }

        }
    }
}
