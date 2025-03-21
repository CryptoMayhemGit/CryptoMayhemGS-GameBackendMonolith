using Dapper;
using FluentAssertions;
using Mayhem.PathWorker.Repository.IntegrationTest.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest
{
    internal class NotificationRepositoryTests : BaseRepositoryTests
    {
        private INotificationRepository notificationRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            notificationRepository = GetNotificationRepository();
        }

        [Test]
        public async Task GetOldNotSentNotyfications_WhenNotificationExists_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addNotification = $"insert into dbo.Notification(Attempts, Email, WalletAddress, WasSent, CreationDate) values (1,'test@email.com', '123gds',0, DATEADD(minute, -500, GETUTCDATE()));";
                await db.QueryAsync(addNotification);
                IEnumerable<NotificationDto> result = await notificationRepository.GetOldNotSentNotyficationsAsync();

                string deleteNotification = "delete from dbo.Notification;";
                await db.QueryAsync(deleteNotification);

                string getNotification = "select * from dbo.Notification";
                IEnumerable<dynamic> notifications = await db.QueryAsync(getNotification);

                result.Should().HaveCount(1);
                notifications.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateNotificationAttempt_WhenNotificationUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int attempts = 1;
                string addNotification = $"insert into dbo.Notification(Attempts, Email, WalletAddress, WasSent, CreationDate) values ({attempts},'test@email.com', '123gds',0, DATEADD(minute, -500, GETUTCDATE())); SELECT CAST(SCOPE_IDENTITY() as int)";
                int notificationId = await db.QuerySingleOrDefaultAsync<int>(addNotification);
                await notificationRepository.UpdateNotificationAttemptAsync(notificationId);
                IEnumerable<NotificationDto> result = await notificationRepository.GetOldNotSentNotyficationsAsync();

                string deleteNotification = "delete from dbo.Notification;";
                await db.QueryAsync(deleteNotification);

                string getNotification = "select * from dbo.Notification";
                IEnumerable<dynamic> notifications = await db.QueryAsync(getNotification);

                result.Should().HaveCount(1);
                result.ToList().First().Attempts.Should().Be(attempts + 1);
                notifications.Should().HaveCount(0);
            }
        }
    }
}
