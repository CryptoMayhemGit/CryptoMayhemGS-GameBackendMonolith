using Dapper;
using FluentAssertions;
using Mayhem.Notification.Repository.IntegrationTests.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Notification.Repository.IntegrationTests
{
    internal class NotificationRepositoryTests : RepositoryBaseTest
    {
        private INotificationRepository notificationRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            notificationRepository = GetNotificationRepository();
        }

        [Test]
        public async Task UpdateNotificationAttempt_WhenNotificationUpdated_ThenGetNumberOfAttempts_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addNitification = "insert into [dbo].[Notification] (WalletAddress, Email, WasSent, CreationDate, LastModificationDate, Attempts) values ('123dsf', 'test@adria.com', 0, GETUTCDATE(), null, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                int notificationId = await db.QuerySingleAsync<int>(addNitification);
                await notificationRepository.UpdateNotificationAttemptAsync(notificationId);
                string getNotificationAttempts = $"select attempts from dbo.Notification where id = {notificationId};";
                int attempts = await db.QuerySingleAsync<int>(getNotificationAttempts);
                string removeNotification = $"delete from dbo.notification where id = {notificationId}";
                await db.QueryAsync(removeNotification);
                string getNotification = $"select * from dbo.notification where id = {notificationId}";
                IEnumerable<dynamic> result = await db.QueryAsync(getNotification);

                attempts.Should().Be(1);
                result.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task GetNotSendedNotyfications_WhenNotificationExists_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string removeNotSentNotifications = $"delete from dbo.notification where WasSent = 0;";
                string getNotSentNotification = $"select * from dbo.notification where WasSent = 0;";

                await db.QuerySingleAsync<int>($"insert into [dbo].[Notification] (WalletAddress, Email, WasSent, CreationDate, LastModificationDate, Attempts) values ('{Guid.NewGuid()}', '{Guid.NewGuid()}', 0, DATEADD(day, -10, GETUTCDATE()), null, 0); SELECT CAST(SCOPE_IDENTITY() as int)");
                await db.QuerySingleAsync<int>($"insert into [dbo].[Notification] (WalletAddress, Email, WasSent, CreationDate, LastModificationDate, Attempts) values ('{Guid.NewGuid()}', '{Guid.NewGuid()}', 0, DATEADD(day, -10, GETUTCDATE()), null, 0); SELECT CAST(SCOPE_IDENTITY() as int)");
                await db.QuerySingleAsync<int>($"insert into [dbo].[Notification] (WalletAddress, Email, WasSent, CreationDate, LastModificationDate, Attempts) values ('{Guid.NewGuid()}', '{Guid.NewGuid()}', 0, DATEADD(day, -10, GETUTCDATE()), null, 0); SELECT CAST(SCOPE_IDENTITY() as int)");

                IEnumerable<NotificationDto> notifications = await notificationRepository.GetOldNotSentNotyficationsAsync();

                await db.QueryAsync(removeNotSentNotifications);
                IEnumerable<dynamic> result = await db.QueryAsync(getNotSentNotification);

                notifications.Should().HaveCount(3);
                result.Should().HaveCount(0);
            }
        }
    }
}
