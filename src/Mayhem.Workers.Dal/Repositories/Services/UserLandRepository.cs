using Dapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.SqlDapper;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Services
{
    public class UserLandRepository : IUserLandRepository
    {
        private readonly string mayhemConnectionString;

        public UserLandRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }

        public async Task<IEnumerable<LandPositionDto>> GetUserLandsFromUserPerspectiveAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<LandPositionDto>(SqlQuerries.GetUserLandsFromUserPerspectiveSql, new { UserId = userId });
            }
        }

        public async Task<UserLandDto> GetUserLandAsync(int userId, long landId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QuerySingleOrDefaultAsync<UserLandDto>(SqlQuerries.GetUserLandWhereUserIdAndLandIdSql, new { UserId = userId, LandId = landId });
            }
        }

        public async Task UpdateUserLandStatusAsync(int id, LandsStatus status)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.UpdateUserLandStatusSql, new { Id = id, Status = (int)status });
            }
        }

        public async Task AddUserLandAsync(UserLandDto userLand)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.AddUserLandSql, new { userLand.LandId, userLand.UserId, Status = (int)userLand.Status, userLand.HasFog, userLand.Owned, userLand.OnSale });
            }
        }
    }
}
