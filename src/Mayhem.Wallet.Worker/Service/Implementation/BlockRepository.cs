using Dapper;
using Mayhem.Configuration.Classes;
using Mayhem.Wallet.Worker.Definitions;
using Mayhem.Wallet.Worker.Service.Interface;
using Mayhem.Worker.Dal.Dto;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Implementation
{
    public class BlockRepository : IBlockRepository
    {
        private readonly string mayhemConnectionString;

        public BlockRepository()
        {
            mayhemConnectionString = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString);
        }

        public async Task<BlockDto> GetLastBlockAsync()
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryFirstOrDefaultAsync<BlockDto>(SqlQuerries.GetTopOneBlockWhereeBlobkTypeIdSql);
            }
        }

        public async Task UpdateLastBlockAsync(long newBlockValue)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.UpdateBlockWhereLasBlockSql, new { LastBlock = newBlockValue });
            }
        }
    }
}
