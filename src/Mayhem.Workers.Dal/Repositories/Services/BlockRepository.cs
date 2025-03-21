using Dapper;
using Mayhem.Blockchain.Enums;
using Mayhem.Configuration.Interfaces;
using Mayhem.SqlDapper;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Services
{
    public class BlockRepository : IBlockRepository
    {
        private readonly string mayhemConnectionString;

        public BlockRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }

        public async Task<BlockDto> GetLastBlockAsync(BlocksType blockType)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryFirstOrDefaultAsync<BlockDto>(SqlQuerries.GetTopOneBlockWhereeBlobkTypeIdSql, new { BlockTypeId = blockType });
            }
        }

        public async Task UpdateLastBlockAsync(long newBlockValue, BlocksType blockType)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.UpdateBlockWhereLasBlockSql, new { BlockTypeId = blockType, LastBlock = newBlockValue });
            }
        }
    }
}
