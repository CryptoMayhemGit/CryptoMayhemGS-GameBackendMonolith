using Mayhem.Nft.Worker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Nft.Worker.Repository.Interface
{
    public interface IGameUserRepository
    {
        Task AddGameUserAsync(List<GameUser> gameUsers);
        Task DeleteAllGameUsersByInvestorCategory(InvestorCategory investorCategory);
        Task<int> GetVoteCategoryIdByInvestorCategory(InvestorCategory investorCategory);
    }
}
