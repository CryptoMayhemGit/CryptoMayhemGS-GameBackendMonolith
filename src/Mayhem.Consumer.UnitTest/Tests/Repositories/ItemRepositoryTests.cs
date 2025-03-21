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
    public class ItemRepositoryTests : BaseRepositoryTests
    {
        private const long expectedNftId = 5;
        private const string expectedWalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D";

        [Test]
        public async Task RemoveItemFromUserAsync_WhenItemExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.RemoveItemFromUserAsync(expectedNftId);
            itemRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task RemoveItemFromUserAsync_WhenItemNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft item with id = {expectedNftId}.";

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.RemoveItemFromUserAsync(expectedNftId);
            itemRepositoryLoggerMock.VerifyResult(Times.Once());
            itemRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task RemoveItemFromUserAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during RemoveItemFromUserAsync.";

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.RemoveItemFromUserAsync(expectedNftId);
            itemRepositoryLoggerMock.VerifyResult(Times.Once());
            itemRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateItemOwnerAsync_WhenItemAndUserExist_ThenGetTrue_Test()
        {
            IdDto expectedResult = new();

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync(expectedResult)
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.UpdateItemOwnerAsync(expectedNftId, expectedWalletAddress);
            itemRepositoryLoggerMock.VerifyResult(Times.Never());
            result.Should().BeTrue();
        }

        [Test]
        public async Task UpdateItemOwnerAsync_WhenItemNotExist_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Cannot find nft item with id = {expectedNftId}.";

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync<IdDto>(null)
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.UpdateItemOwnerAsync(expectedNftId, expectedWalletAddress);
            itemRepositoryLoggerMock.VerifyResult(Times.Once());
            itemRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateItemOwnerAsync_WhenUserNotExist_ThenGetFalse_Test()
        {
            IdDto expectedResult = new();
            string expectedErrorMessage = $"Cannot find user with wallet address = {expectedWalletAddress}.";

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsync(expectedResult)
                .WithQueryFirstAsync<IdDto>(null)
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.UpdateItemOwnerAsync(expectedNftId, expectedWalletAddress);
            itemRepositoryLoggerMock.VerifyResult(Times.Once());
            itemRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }

        [Test]
        public async Task UpdateItemOwnerAsync_WhenThrownError_ThenGetFalse_Test()
        {
            string expectedErrorMessage = $"Error occurred during UpdateItemOwnerAsync.";

            Mock<ILogger<ItemRepository>> itemRepositoryLoggerMock = LoggerMockBuilder
                .Create<ItemRepository>();

            IDapperWrapper dapperWrapper = DapperWrapperMockBuilder
                .Create()
                .WithQueryFirstOrDefaultAsyncThrowError<IdDto>()
                .Build();

            IItemRepository home = new ItemRepository(mayhemConfiguration, dapperWrapper, itemRepositoryLoggerMock.Object);

            bool result = await home.UpdateItemOwnerAsync(expectedNftId, expectedWalletAddress);
            itemRepositoryLoggerMock.VerifyResult(Times.Once());
            itemRepositoryLoggerMock.VerifyResult(expectedErrorMessage);
            result.Should().BeFalse();
        }
    }
}