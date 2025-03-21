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
    public class LandRepositoryTests : BaseRepositoryTests
    {
        private const long expectedNftId = 5;
        private const string expectedWalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D";

        [Test]
        public async Task RemoveLandFromUserAsync_WhenLandExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.RemoveLandFromUserAsync(expectedNftId);
            landRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task RemoveLandFromUserAsync_WhenLandNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft land with id = {expectedNftId}.";

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.RemoveLandFromUserAsync(expectedNftId);
            landRepositoryLoggerMock.VerifyResult(Times.Once());
            landRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task RemoveLandFromUserAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during RemoveLandFromUserAsync.";

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.RemoveLandFromUserAsync(expectedNftId);
            landRepositoryLoggerMock.VerifyResult(Times.Once());
            landRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateLandOwnerAsync_WhenLandAndUserExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync(expectedResult)
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.UpdateLandOwnerAsync(expectedNftId, expectedWalletAddress);
            landRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task UpdateLandOwnerAsync_WhenLandNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft land with id = {expectedNftId}.";

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.UpdateLandOwnerAsync(expectedNftId, expectedWalletAddress);
            landRepositoryLoggerMock.VerifyResult(Times.Once());
            landRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateLandOwnerAsync_WhenUserNotExist_ThenGetFalse_Test()
        {
            IdDto expectedResult = new();
            string expectedErrorMessage = $"Cannot find user with wallet address = {expectedWalletAddress}.";

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync<IdDto>(null)
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.UpdateLandOwnerAsync(expectedNftId, expectedWalletAddress);
            landRepositoryLoggerMock.VerifyResult(Times.Once());
            landRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateLandOwnerAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during UpdateLandOwnerAsync.";

            Mock<ILogger<LandRepository>> landRepositoryLoggerMock = LoggerMockBuilder
                .Create<LandRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            ILandRepository home = new LandRepository(mayhemConfiguration, dapperWrapper, landRepositoryLoggerMock.Object);

            bool result = await home.UpdateLandOwnerAsync(expectedNftId, expectedWalletAddress);
            landRepositoryLoggerMock.VerifyResult(Times.Once());
            landRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }
    }
}