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
    internal class NpcRepositoryTests : RepositoryBaseTest
    {
        private INpcRepository npcRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            npcRepository = GetNpcRepository();
        }

        [Test]
        public async Task RemoveNpcFromUser_WhenNpcRemoved_ThenGetTrue_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into [dbo].[GameUser](Email, WalletAddress) values ('test@email.com', 'some address'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);
                string addNpcToDb = $"insert into [nft].[Npc](UserId, Name, Address, NpcTypeId, NpcHealthStateId, IsAvatar, IsMinted, NpcStatusId) values ({userId}, 'npc name', '{Guid.NewGuid()}', 1,1,1,1,1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpcToDb);
                bool result = await npcRepository.RemoveNpcFromUserAsync(npcId);
                string removeNpc = $"delete from [nft].[Npc]";
                await db.QueryAsync(removeNpc);
                string removeUser = $"delete from [dbo].[GameUser]";
                await db.QueryAsync(removeUser);

                string getUser = $"select * from [dbo].[GameUser] where id = {userId}";
                string getNpc = $"select * from [nft].[Npc] where id = {npcId}";
                IEnumerable<dynamic> userResult = await db.QueryAsync(getUser);
                IEnumerable<dynamic> npcResult = await db.QueryAsync(getNpc);

                result.Should().BeTrue();
                userResult.Should().HaveCount(0);
                npcResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateNpcOwnerAsync_WhenNpcOwnerUpdated_ThenGetTrue_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string walletAddres1 = Guid.NewGuid().ToString();
                string walletAddres2 = Guid.NewGuid().ToString();
                string addUser1 = $"insert into [dbo].[GameUser](Email, WalletAddress) values ('test1@email.com', '{walletAddres1}'); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUser2 = $"insert into [dbo].[GameUser](Email, WalletAddress) values ('test2@email.com', '{walletAddres2}'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int user1Id = await db.QuerySingleAsync<int>(addUser1);
                int user2Id = await db.QuerySingleAsync<int>(addUser2);
                string addNpcToDb = $"insert into [nft].[Npc](UserId, Name, Address, NpcTypeId, NpcHealthStateId, IsAvatar, IsMinted, NpcStatusId) values ({user1Id}, 'npc name', '{Guid.NewGuid()}', 1,1,1,1,1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpcToDb);

                bool result = await npcRepository.UpdateNpcOwnerAsync(npcId, walletAddres2);

                string getNpc = $"select * from [nft].[Npc] where id = {npcId}";
                IEnumerable<Npc> npcWithNewOwner = await db.QueryAsync<Npc>(getNpc);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string removeUser1 = $"delete from [dbo].[GameUser] where id = {user1Id}";
                string removeUser2 = $"delete from [dbo].[GameUser] where id = {user2Id}";
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(removeUser1);
                await db.QueryAsync(removeUser2);

                string getUser1 = $"select * from [dbo].[GameUser] where id = {user1Id}";
                string getUser2 = $"select * from [dbo].[GameUser] where id = {user2Id}";
                IEnumerable<dynamic> userResult1 = await db.QueryAsync(getUser1);
                IEnumerable<dynamic> userResult2 = await db.QueryAsync(getUser2);
                IEnumerable<dynamic> npcResult = await db.QueryAsync(getNpc);

                result.Should().BeTrue();
                userResult1.Should().HaveCount(0);
                userResult2.Should().HaveCount(0);
                npcResult.Should().HaveCount(0);
                npcWithNewOwner.First().UserId.Should().Be(user2Id);
            }
        }
    }

    internal class Npc
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}