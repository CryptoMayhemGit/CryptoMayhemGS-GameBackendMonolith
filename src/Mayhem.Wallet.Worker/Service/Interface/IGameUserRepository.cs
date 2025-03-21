using Mayhem.Wallet.Worker.Models;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Interface
{
    /// <summary>
    /// Game User Repository
    /// </summary>
    public interface IGameUserRepository
    {
        /// <summary>
        /// Add new wallet.
        /// </summary>
        /// <param name="newWallet">The new wallet value.</param
        /// <param name="voteCategoryId">Id of category.</param>
        /// <param name="usdcAmount">Count of USDC.</param>
        /// <returns></returns>
        Task AddWalletAsync(string newWallet, int voteCategoryId, int usdcAmount);

        Task<int> GetVoteCategoryIdByInvestorCategory(InvestorCategory investorCategory);
    }
}
