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
    public class NpcRepositoryTests : BaseRepositoryTests
    {
        private const long expectedNftId = 5;
        private const string expectedWalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D";

        [Test]
        public async Task RemoveNpcFromUserAsync_WhenNpcExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.RemoveNpcFromUserAsync(expectedNftId);
            npcRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task RemoveNpcFromUserAsync_WhenNpcNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft npc with id = {expectedNftId}.";

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.RemoveNpcFromUserAsync(expectedNftId);
            npcRepositoryLoggerMock.VerifyResult(Times.Once());
            npcRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task RemoveNpcFromUserAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during RemoveNpcFromUserAsync.";

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.RemoveNpcFromUserAsync(expectedNftId);
            npcRepositoryLoggerMock.VerifyResult(Times.Once());
            npcRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateNpcOwnerAsync_WhenNpcAndUserExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync(expectedResult)
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.UpdateNpcOwnerAsync(expectedNftId, expectedWalletAddress);
            npcRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task UpdateNpcOwnerAsync_WhenNpcNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft npc with id = {expectedNftId}.";

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.UpdateNpcOwnerAsync(expectedNftId, expectedWalletAddress);
            npcRepositoryLoggerMock.VerifyResult(Times.Once());
            npcRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateNpcOwnerAsync_WhenUserNotExist_ThenGetFalse_Test()
        {
            IdDto expectedResult = new();
            string expectedErrorMessage = $"Cannot find user with wallet address = {expectedWalletAddress}.";

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync<IdDto>(null)
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.UpdateNpcOwnerAsync(expectedNftId, expectedWalletAddress);
            npcRepositoryLoggerMock.VerifyResult(Times.Once());
            npcRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateNpcOwnerAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during UpdateNpcOwnerAsync.";

            Mock<ILogger<NpcRepository>> npcRepositoryLoggerMock = LoggerMockBuilder
                .Create<NpcRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            INpcRepository home = new NpcRepository(mayhemConfiguration, dapperWrapper, npcRepositoryLoggerMock.Object);

            bool result = await home.UpdateNpcOwnerAsync(expectedNftId, expectedWalletAddress);
            npcRepositoryLoggerMock.VerifyResult(Times.Once());
            npcRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }
    }
}