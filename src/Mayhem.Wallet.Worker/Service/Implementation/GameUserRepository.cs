using Dapper;
using Mayhem.Configuration.Classes;
using Mayhem.Wallet.Worker.Definitions;
using Mayhem.Wallet.Worker.Models;
using Mayhem.Wallet.Worker.Service.Interface;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Implementation
{
    public class GameUserRepository : IGameUserRepository
    {
        private readonly string mayhemConnectionString;

        public GameUserRepository()
        {
            mayhemConnectionString = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString);
        }

        public async Task AddWalletAsync(string newWallet, int voteCategoryId, int usdcAmount)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.AddWalletSql, new
                {
                    Wallet = newWallet,
                    VoteCategoryId = voteCategoryId,
                    UsdcAmount = usdcAmount
                });
            }
        }

        public async Task<int> GetVoteCategoryIdByInvestorCategory(InvestorCategory investorCategory)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryFirstOrDefaultAsync<int>(SqlQuerries.GetVoteCategoryIdByInvestorCategory, new { InvestorCategory = investorCategory.ToString() });
            }
        }
    }
}
