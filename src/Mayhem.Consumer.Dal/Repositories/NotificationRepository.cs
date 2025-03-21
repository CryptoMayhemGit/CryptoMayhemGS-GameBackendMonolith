using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Consumer.Dal.Dto.Dtos;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Mayhem.Messages;
using Mayhem.SqlDapper;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string mayhemConnectionString;
        private readonly ILogger<NotificationRepository> logger;
        private readonly NotificationConfigruation notificationConfigruation;
        private readonly IDapperWrapper dapperWrapper;

        public NotificationRepository(
            IMayhemConfigurationService mayhemConfigurationService,
            ILogger<NotificationRepository> logger,
            IDapperWrapper dapperWrapper)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
            notificationConfigruation = mayhemConfigurationService.MayhemConfiguration.NotificationConfigruation;
            this.dapperWrapper = dapperWrapper;
            this.logger = logger;
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(long notificationId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    NotificationDto notifications = await dapperWrapper.QueryFirstOrDefaultAsync<NotificationDto>(db, SqlQuerries.GetTopOneNotificationWhereIdSql, new { Id = notificationId });

                    if (notifications == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNotificationWithId(notificationId));
                    }

                    return await Task.FromResult(notifications);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(GetNotificationByIdAsync)));
                    return null;
                }
            }
        }

        public async Task<bool> SetNotificationAsSentAsync(long notificationId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateNotificationSql, new { Id = notificationId });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(SetNotificationAsSentAsync)));
                    return await Task.FromResult(false);
                }

                return await Task.FromResult(true);
            }
        }
    }
}
