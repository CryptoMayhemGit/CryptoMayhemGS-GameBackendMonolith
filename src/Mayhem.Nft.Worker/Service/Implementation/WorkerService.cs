using Mayhem.Nft.Worker.Models;
using Mayhem.Nft.Worker.Repository.Interface;
using Mayhem.Nft.Worker.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Nft.Worker.Service.Implementation
{
    public class WorkerService : IWorkerService
    {
        private readonly IGameUserRepository _gameUserRepository;
        private readonly IBlockchainService _blockchainService;
        private readonly ILogger<WorkerService> log;

        public WorkerService(ILogger<WorkerService> log, IGameUserRepository gameUserRepository, IBlockchainService blockchainService)
        {
            this.log = log;
            this._gameUserRepository = gameUserRepository;
            this._blockchainService = blockchainService;
        }
        
        public async Task UpdateAsync()
        {
            log.LogInformation("Start update");
            List<GameUser> walletAddresses = new();
            int voteCategoryId = await _gameUserRepository.GetVoteCategoryIdByInvestorCategory(InvestorCategory.TGLPNFT);
            int numberOfNFTCollection = 100;
            
            try
            {
                for (int i = 1; i <= numberOfNFTCollection; i++)
                {
                    string owner = await _blockchainService.GetOwnerOf(i);
                    if (!owner.Equals(string.Empty))
                        walletAddresses.Add(new GameUser { Wallet = owner, VoteCategoryId = voteCategoryId });
                }

                await _gameUserRepository.DeleteAllGameUsersByInvestorCategory(InvestorCategory.TGLPNFT);
                await _gameUserRepository.AddGameUserAsync(walletAddresses);
            }
            catch(Exception ex)
            {
                log.LogError($"Error message: {ex.Message}. StackTrace: {ex.StackTrace}");
            }

            log.LogInformation("Success update");
        }
    }
}
