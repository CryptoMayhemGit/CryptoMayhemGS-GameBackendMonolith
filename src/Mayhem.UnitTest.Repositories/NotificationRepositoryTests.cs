using FluentAssertions;
using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class NotificationRepositoryTests : UnitTestBase
    {
        private INotificationRepository notificationRepository;
        private IMayhemDataContext mayhemDataContext;

        [SetUp]
        public void Setup()
        {
            notificationRepository = GetService<INotificationRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test, Order(1)]
        public async Task AddNotificationAsync_WhenNotificationCreated_ThenReturnTrue_Test()
        {
            SendActivationNotificationCommandRequestDto sendActivationNotificationCommandRequestDto = new()
            {
                Wallet = "0x7081571a87dA302B57311C99C0f369c8fAf18734",
                Email = "TakiTam@email.com"
            };

            int? notificationId = await notificationRepository.AddNotificationAsync(sendActivationNotificationCommandRequestDto);

            List<Notification> notificationList = mayhemDataContext.Notifications
                .Where(x =>
                x.WalletAddress.Equals(sendActivationNotificationCommandRequestDto.Wallet)
                && x.Email.Equals(sendActivationNotificationCommandRequestDto.Email)).ToList();

            notificationId.Should().HaveValue();
            notificationList.Should().HaveCount(1);
            notificationList[0].Id.Should().Be(notificationId);
            notificationList[0].WalletAddress.Should().Be(sendActivationNotificationCommandRequestDto.Wallet);
            notificationList[0].Email.Should().Be(sendActivationNotificationCommandRequestDto.Email);
        }

        [Test, Order(2)]
        public async Task GetNotificationByEmail_WhenNotificationExist_ThenGetIt_Test()
        {
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@email.com";
            await mayhemDataContext.Notifications.AddAsync(new Notification()
            {
                Email = email
            });
            await mayhemDataContext.SaveChangesAsync();

            NotificationDto notification = await notificationRepository.GetNotificationByEmailAsync(email);

            notification.Should().NotBeNull();
            notification.Email.Should().Be(email);
        }

        [Test, Order(3)]
        public async Task UpdateNotificationDate_WhenNotificationExist_ThenUpdateIt_Test()
        {
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@email.com";
            EntityEntry<Notification> notification = await mayhemDataContext.Notifications.AddAsync(new Notification()
            {
                Email = email
            });
            await mayhemDataContext.SaveChangesAsync();

            await notificationRepository.UpdateNotificationDate(notification.Entity.Id);
            Notification notificationDb = await mayhemDataContext.Notifications.SingleOrDefaultAsync(x => x.Email == email);

            notificationDb.Should().NotBeNull();
            notificationDb.Email.Should().Be(email);
            notificationDb.LastModificationDate.Value.Ticks.Should().BeGreaterThan(DateTime.UtcNow.AddMinutes(-1).Ticks);
        }


        [Test, Order(8)]
        public async Task CheckActivationLinkAsync_WhenNotificationRemoved_ThenReturnTrue_Test()
        {
            NotificationDataDto notificationData = new()
            {
                Wallet = "0x7081571a87dA302B57311C99C0f369c8fAf18734",
                Email = "TakiTam@email.com",
                CreationDate = DateTime.UtcNow
            };

            Notification notificationToChange = mayhemDataContext.Notifications
                .SingleOrDefault(x =>
                x.WalletAddress.Equals(notificationData.Wallet)
                && x.Email.Equals(notificationData.Email));

            notificationToChange.WasSent = true;
            await mayhemDataContext.SaveChangesAsync();

            bool result = await notificationRepository.CheckActivationLinkAsync(notificationData.Wallet, notificationData.Email);

            List<Notification> notificationList = mayhemDataContext.Notifications
                .Where(x =>
                x.WalletAddress.Equals(notificationData.Wallet)
                && x.Email.Equals(notificationData.Email)).ToList();

            result.Should().BeTrue();
            notificationList.Should().HaveCount(0);
        }
    }
}