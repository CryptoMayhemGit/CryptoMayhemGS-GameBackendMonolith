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
    public class DiscoveryMissionRepository : IDiscoveryMissionRepository
    {
        private readonly string mayhemConnectionString;

        public DiscoveryMissionRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }

        public async Task<IEnumerable<DiscoveryMissionDto>> GetFinishedMissionsAsync()
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<DiscoveryMissionDto>(SqlQuerries.GetDicoveryMissionsWhereFinishDateSql);
            }
        }

        public async Task RemoveMissionAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.DeleteDiscoveryMissionWhereIdSql, new { Id = id });
            }
        }
    }
}
