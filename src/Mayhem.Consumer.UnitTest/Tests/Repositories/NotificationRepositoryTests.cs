using FluentAssertions;
using Mayhem.Consumer.Dal.Dto.Dtos;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Mayhem.Consumer.Dal.Repositories;
using Mayhem.Consumer.Test.Common.Builder;
using Mayhem.Consumer.UnitTest.Base;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.Consumer.UnitTest.Tests.Repositories
{
    public class NotificationRepositoryTests : BaseRepositoryTests
    {
        private const int expectedNftId = 5;
        private const string expectedWalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D";
        private const string expectedEmailAddress = "invalid@AdriaGames.com";

        [Test]
        public async Task GetNotificationByIdAsync_WhenNotificationExist_ThenGetTrue_Test()
        {
            NotificationDto expectedResult = new() { Id = expectedNftId, WalletAddress = expectedWalletAddress, Email = expectedEmailAddress };

            Mock<ILogger<NotificationRepository>> notificationRepositoryLoggerMock = LoggerMockBuilder
                .Create<NotificationRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .Build();

            INotificationRepository home = new NotificationRepository(mayhemConfiguration, notificationRepositoryLoggerMock.Object, dapperWrapper);

            NotificationDto notificationDtoResult = await home.GetNotificationByIdAsync(expectedNftId);
            notificationRepositoryLoggerMock.VerifyResult(Times.Never());
            notificationDtoResult.Email.Should().Be(expectedEmailAddress);
            notificationDtoResult.WalletAddress.Should().Be(expectedWalletAddress);
            notificationDtoResult.Id.Should().Be(expectedNftId);
        }

        [Test]
        public async Task GetNotificationByIdAsync_WhenNotificationNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find notification with id = {expectedNftId}.";

            Mock<ILogger<NotificationRepository>> notificationRepositoryLoggerMock = LoggerMockBuilder
                .Create<NotificationRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<NotificationDto>(null)
                .Build();

            INotificationRepository home = new NotificationRepository(mayhemConfiguration, notificationRepositoryLoggerMock.Object, dapperWrapper);

            NotificationDto notificationDtoResult = await home.GetNotificationByIdAsync(expectedNftId);
            notificationRepositoryLoggerMock.VerifyResult(Times.Once());
            notificationRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            notificationDtoResult.Should().BeNull();
        }

        [Test]
        public async Task GetNotificationByIdAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during GetNotificationByIdAsync.";

            Mock<ILogger<NotificationRepository>> notificationRepositoryLoggerMock = LoggerMockBuilder
                .Create<NotificationRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<NotificationDto>()
                .Build();

            INotificationRepository home = new NotificationRepository(mayhemConfiguration, notificationRepositoryLoggerMock.Object, dapperWrapper);

            NotificationDto notificationDtoResult = await home.GetNotificationByIdAsync(expectedNftId);
            notificationRepositoryLoggerMock.VerifyResult(Times.Once());
            notificationRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            notificationDtoResult.Should().BeNull();
        }

        [Test]
        public async Task SetNotificationAsSentAsync_WhenNotificationExist_ThenGetTrue_Test()
        {
            Mock<ILogger<NotificationRepository>> notificationRepositoryLoggerMock = LoggerMockBuilder
                .Create<NotificationRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryAsync()
                .Build();

            INotificationRepository home = new NotificationRepository(mayhemConfiguration, notificationRepositoryLoggerMock.Object, dapperWrapper);

            bool result = await home.SetNotificationAsSentAsync(expectedNftId);
            notificationRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task SetNotificationAsSentAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during SetNotificationAsSentAsync.";

            Mock<ILogger<NotificationRepository>> notificationRepositoryLoggerMock = LoggerMockBuilder
                .Create<NotificationRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryAsyncThrowError()
                .Build();

            INotificationRepository home = new NotificationRepository(mayhemConfiguration, notificationRepositoryLoggerMock.Object, dapperWrapper);

            bool result = await home.SetNotificationAsSentAsync(expectedNftId);
            notificationRepositoryLoggerMock.VerifyResult(Times.Once());
            notificationRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }
    }
}