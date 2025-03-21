using Dapper;
using FluentAssertions;
using Mayhem.PathWorker.Repository.IntegrationTest.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest
{
    internal class LandRepositoryTests : BaseRepositoryTests
    {
        private ILandRepository landRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            landRepository = GetLandRepository();
        }

        [Test]
        public async Task GetUserLands_WhenUserLandExists_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser1 = "insert into dbo.GameUser (Email, WalletAddress) values ('test1@email.com', '123jlk1j21'); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUser2 = "insert into dbo.GameUser (Email, WalletAddress) values ('test2@email.com', '123jlk1j22'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId1 = await db.QuerySingleAsync<int>(addUser1);
                int userId2 = await db.QuerySingleAsync<int>(addUser2);
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                string addNpc1 = $"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId1},'Name', '123kfg', null, 1, 1, 1, null, 1, {landId1}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addNpc2 = $"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId2},'Name', '123kfg', null, 1, 1, 1, null, 1, {landId2}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";

                int npc1Id = await db.QuerySingleAsync<int>(addNpc1);
                int npc2Id = await db.QuerySingleAsync<int>(addNpc2);
                string addUserLand1 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId1}, {userId1}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand2 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId1}, {userId2}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId1 = await db.QuerySingleAsync<int>(addUserLand1);
                int userLandId2 = await db.QuerySingleAsync<int>(addUserLand2);

                List<UserLandNpcDto> result = (await landRepository.GetLandNpcsAsync(landId1)).ToList();

                string removeNpcs = $"delete from [nft].[Npc]";
                string removeLands = $"delete from [nft].[Land]";
                string removeUsers = $"delete from [dbo].[GameUser]";
                string removeUserLands = $"delete from [dbo].[UserLand]";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                await db.QueryAsync(removeUserLands);
                await db.QueryAsync(removeNpcs);
                await db.QueryAsync(removeLands);
                await db.QueryAsync(removeLandInstance);
                await db.QueryAsync(removeUsers);

                string getUser = $"select * from [dbo].[GameUser]";
                string getLand = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getUserLands = $"select * from [dbo].[UserLand]";
                IEnumerable<dynamic> userResult = await db.QueryAsync(getUser);
                IEnumerable<dynamic> landResult = await db.QueryAsync(getLand);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> userLandResult = await db.QueryAsync(getUserLands);

                result.Should().HaveCount(2);
                result[0].LandUserId.Should().Be(userId1);
                result[1].LandUserId.Should().Be(userId2);
                result[0].NpcUserId.Should().Be(userId1);
                result[1].NpcUserId.Should().Be(userId1);
                userResult.Should().HaveCount(0);
                landResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                userLandResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateLandHasFogAsFalse_WhenLandUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test@email.com', '123jlk1j2'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                string addUseLand = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId}, {userId}, 3, 1, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId = await db.QuerySingleAsync<int>(addUseLand);
                await landRepository.RemoveFogFromLandsAsync(landId, userId);

                string getUserLand = $"select HasFog from dbo.userland where id = {userLandId}";
                bool result = await db.QuerySingleAsync<bool>(getUserLand);

                string removeLands = $"delete from [nft].[Land]";
                string removeUsers = $"delete from [dbo].[GameUser]";
                string removeUserLands = $"delete from [dbo].[UserLand]";
                string removeLandInstance = $"delete from [dbo].[LandInstance]";
                await db.QueryAsync(removeUserLands);
                await db.QueryAsync(removeLands);
                await db.QueryAsync(removeUsers);
                await db.QueryAsync(removeLandInstance);

                string getUser = $"select * from [dbo].[GameUser] where id = {userId}";
                string getLand = $"select * from [nft].[Land] where id = {landId}";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getUserLands = $"select * from [dbo].[UserLand]";
                IEnumerable<dynamic> userResult = await db.QueryAsync(getUser);
                IEnumerable<dynamic> landResult = await db.QueryAsync(getLand);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> userLandResult = await db.QueryAsync(getUserLands);

                result.Should().Be(false);
                userResult.Should().HaveCount(0);
                landResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                userLandResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateLandHasFogAsTrue_WhenLandUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test@email.com', '123jlk1j2'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                string addUseLand = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId}, {userId}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId = await db.QuerySingleAsync<int>(addUseLand);
                await landRepository.AddFogToLandsAsync(landId, userId, 1);

                string getUserLand = $"select HasFog from dbo.userland where id = {userLandId}";
                bool result = await db.QuerySingleAsync<bool>(getUserLand);

                string removeLands = $"delete from [nft].[Land]";
                string removeUsers = $"delete from [dbo].[GameUser]";
                string removeUserLands = $"delete from [dbo].[UserLand]";
                string removeLandInstance = $"delete from [dbo].[LandInstance]";
                await db.QueryAsync(removeUserLands);
                await db.QueryAsync(removeLands);
                await db.QueryAsync(removeUsers);
                await db.QueryAsync(removeLandInstance);

                string getUser = $"select * from [dbo].[GameUser] where id = {userId}";
                string getLand = $"select * from [nft].[Land] where id = {landId}";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getUserLands = $"select * from [dbo].[UserLand]";
                IEnumerable<dynamic> userResult = await db.QueryAsync(getUser);
                IEnumerable<dynamic> landResult = await db.QueryAsync(getLand);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> userLandResult = await db.QueryAsync(getUserLands);

                result.Should().Be(true);
                userResult.Should().HaveCount(0);
                landResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                userLandResult.Should().HaveCount(0);
            }
        }
    }
}
