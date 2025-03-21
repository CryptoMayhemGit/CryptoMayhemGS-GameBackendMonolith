using Dapper;
using Mayhem.Nft.Worker.Definitions;
using Mayhem.Nft.Worker.Models;
using Mayhem.Nft.Worker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Nft.Worker.Repository.Implementation
{
    public class GameUserRepository : IGameUserRepository
    {
        private readonly string _mayhemConnectionString;
        
        public GameUserRepository()
        {
            this._mayhemConnectionString = Environment.GetEnvironmentVariable("MayhemAzureAppConfigurationConnecitonString");
        }

        public async Task AddGameUserAsync(List<GameUser> gameUsers)
        {
            using (IDbConnection db = new SqlConnection(_mayhemConnectionString))
            {
                if (db.State == ConnectionState.Closed) db.Open();
                IDbTransaction trans = db.BeginTransaction();
                await db.ExecuteAsync(SqlQuerries.AddGameUser, gameUsers, transaction: trans);
                trans.Commit();
            }
        }
        
        public async Task DeleteAllGameUsersByInvestorCategory(InvestorCategory investorCategory)
        {
            using (IDbConnection db = new SqlConnection(_mayhemConnectionString))
            {
                await db.ExecuteAsync(SqlQuerries.DeleteAllGameUsersByInvestorCategory, new { InvestorCategory = investorCategory.ToString() });
            }
        }

        public async Task<int> GetVoteCategoryIdByInvestorCategory(InvestorCategory investorCategory)
        {
            using (IDbConnection db = new SqlConnection(_mayhemConnectionString))
            {
                return await db.QueryFirstOrDefaultAsync<int>(SqlQuerries.GetVoteCategoryIdByInvestorCategory, new { InvestorCategory = investorCategory.ToString() });
            }
        }
    }
}
