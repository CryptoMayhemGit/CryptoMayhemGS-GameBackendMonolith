using Dapper;
using FluentAssertions;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.IntegrationTest.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Consumer.IntegrationTest
{
    internal class ItemRepositoryTests : RepositoryBaseTest
    {
        private IItemRepository itemRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            itemRepository = GetItemRepository();
        }

        [Test]
        public async Task RemoveItemFromUser_WhenItemRemoved_ThenGetTrue_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into [dbo].[GameUser](Email, WalletAddress) values ('test@email.com', 'some address'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);
                string addItemToDb = $"insert into [nft].[Item](UserId, ItemTypeId, IsUsed, Name, Address, IsMinted) values ({userId}, 1, 0, 'item name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int itemId = await db.QuerySingleAsync<int>(addItemToDb);
                bool result = await itemRepository.RemoveItemFromUserAsync(itemId);
                string removeItem = $"delete from [nft].[Item] where id = {itemId}";
                string removeUser = $"delete from [dbo].[GameUser] where id = {userId}";
                await db.QueryAsync(removeItem);
                await db.QueryAsync(removeUser);

                string getUser = $"select * from [dbo].[GameUser] where id = {userId}";
                string getItem = $"select * from [nft].[Item] where id = {itemId}";
                IEnumerable<dynamic> userResult = await db.QueryAsync(getUser);
                IEnumerable<dynamic> itemResult = await db.QueryAsync(getItem);

                result.Should().BeTrue();
                userResult.Should().HaveCount(0);
                itemResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateItemOwnerAsync_WhenItemOwnerUpdated_ThenGetTrue_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string walletAddres1 = Guid.NewGuid().ToString();
                string walletAddres2 = Guid.NewGuid().ToString();
                string addUser1 = $"insert into [dbo].[GameUser](Email, WalletAddress) values ('test1@email.com', '{walletAddres1}'); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUser2 = $"insert into [dbo].[GameUser](Email, WalletAddress) values ('test2@email.com', '{walletAddres2}'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int user1Id = await db.QuerySingleAsync<int>(addUser1);
                int user2Id = await db.QuerySingleAsync<int>(addUser2);
                string addItemToDb = $"insert into [nft].[Item](UserId, ItemTypeId, IsUsed, Name, Address, IsMinted) values ({user1Id}, 1, 0, 'item name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int itemId = await db.QuerySingleAsync<int>(addItemToDb);

                bool result = await itemRepository.UpdateItemOwnerAsync(itemId, walletAddres2);

                string getItem = $"select * from [nft].[Item] where id = {itemId}";
                IEnumerable<Item> itemWithNewOwner = await db.QueryAsync<Item>(getItem);

                string removeItem = $"delete from [nft].[Item] where id = {itemId}";
                string removeUser1 = $"delete from [dbo].[GameUser] where id = {user1Id}";
                string removeUser2 = $"delete from [dbo].[GameUser] where id = {user2Id}";
                await db.QueryAsync(removeItem);
                await db.QueryAsync(removeUser1);
                await db.QueryAsync(removeUser2);

                string getUser1 = $"select * from [dbo].[GameUser] where id = {user1Id}";
                string getUser2 = $"select * from [dbo].[GameUser] where id = {user2Id}";
                IEnumerable<dynamic> userResult1 = await db.QueryAsync(getUser1);
                IEnumerable<dynamic> userResult2 = await db.QueryAsync(getUser2);
                IEnumerable<dynamic> itemResult = await db.QueryAsync(getItem);

                result.Should().BeTrue();
                userResult1.Should().HaveCount(0);
                userResult2.Should().HaveCount(0);
                itemResult.Should().HaveCount(0);
                itemWithNewOwner.First().UserId.Should().Be(user2Id);
            }
        }
    }

    internal class Item
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
