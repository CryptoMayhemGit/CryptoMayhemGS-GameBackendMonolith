using Dapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.SqlDapper;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Services
{
    public class LandRepository : ILandRepository
    {
        private readonly string mayhemConnectionString;

        public LandRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }

        public async Task<IEnumerable<UserLandNpcDto>> GetLandNpcsAsync(long landId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<UserLandNpcDto>(SqlQuerries.GetLandWithNpcSql, new { Id = landId });
            }
        }

        public async Task AddFogToLandsAsync(long landId, int userId, long npcId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.ExecuteAsync(SqlQuerries.Procedures.AddFogToLandsProcedure, new { LandId = landId, UserId = userId, NpcId = npcId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task RemoveFogFromLandsAsync(long landId, int userId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.ExecuteAsync(SqlQuerries.Procedures.RemoveFogFromLandsProcedure, new { LandId = landId, UserId = userId }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
