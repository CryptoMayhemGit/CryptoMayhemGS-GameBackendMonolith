using Dapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.SqlDapper;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Services
{
    public class TravelRepository : ITravelRepository
    {
        private readonly string mayhemConnectionString;

        public TravelRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
        }

        public async Task AddTravelsAsync(List<TravelDto> travels)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                foreach (TravelDto travel in travels)
                {
                    await db.QueryAsync(SqlQuerries.AddTravelSql, new { travel.NpcId, travel.LandFromId, travel.LandToId, travel.FinishDate, CreationDate = DateTime.UtcNow });
                }
            }
        }

        public async Task<IEnumerable<TravelDto>> GetTravelsAsync()
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<TravelDto>(SqlQuerries.GetTravelsSql);
            }
        }

        public async Task<IEnumerable<TravelDto>> GetTravelsByNpcIdAsync(long npcId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<TravelDto>(SqlQuerries.GetTravelWhereNpcIdSql, new { NpcId = npcId });
            }
        }

        public async Task RemoveTravelAsync(long travelId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync(SqlQuerries.DeleteTravelWhereIdSql, new { Id = travelId });
            }
        }

        public async Task RemoveTravelsByNpcIdAsync(long npcId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync<int>(SqlQuerries.DeleteTravelWhereNpcIdSql, new { NpcId = npcId });
            }
        }
    }
}
