using Dapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.SqlDapper;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Services
{
    public class NpcRepository : INpcRepository
    {
        private readonly string mayhemConnectionString;

        public NpcRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }
        public async Task<NpcDto> GetNpcAsync(long npcId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QuerySingleOrDefaultAsync<NpcDto>(SqlQuerries.GetNpcWhereIdSql, new { Id = npcId });
            }
        }

        public async Task UpdateNpcLandAsync(long id, long landToId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.UpdateNpcLandSql, new { Id = id, LandId = landToId });
            }
        }

        public async Task UpdateNpcStatusAsync(long id, NpcsStatus npcStatusId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.UpdateNpcStatusSql, new { Id = id, NpcStatusId = (int)npcStatusId });
            }
        }
    }
}
