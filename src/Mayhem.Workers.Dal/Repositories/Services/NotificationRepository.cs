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
    public class NotificationRepository : INotificationRepository
    {
        private readonly string mayhemConnectionString;
        private readonly int olderNotificationThenInMinutes;

        public NotificationRepository(IMayhemConfigurationService mayhemConfigurationService)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
            olderNotificationThenInMinutes = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.ResendNotificationOlderThenInMinutes;
        }

        public async Task<IEnumerable<NotificationDto>> GetOldNotSentNotyficationsAsync()
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                return await db.QueryAsync<NotificationDto>(SqlQuerries.GetOldNotSentNotyficationsSql(olderNotificationThenInMinutes));
            }
        }

        public async Task UpdateNotificationAttemptAsync(int notificationId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                await db.QueryAsync<int>(SqlQuerries.UpdateNotyficationAttemptSql, new { @Id = notificationId });
            }
        }
    }
}
