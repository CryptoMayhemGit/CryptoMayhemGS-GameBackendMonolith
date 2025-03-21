using Dapper;
using FluentAssertions;
using Mayhem.PathWorker.Repository.IntegrationTest.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
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
    internal class UserLandRepositoryTests : BaseRepositoryTests
    {
        private IUserLandRepository userLandRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            userLandRepository = GetUserLandRepository();
        }

        [Test]
        public async Task GetUserLandsFromUserPerspective_WhenUserLandExists_ThenGetThem_Test()
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
                string addLand3 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 3, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand4 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 4, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand5 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 5, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand6 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 6, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                int landId3 = await db.QuerySingleAsync<int>(addLand3);
                int landId4 = await db.QuerySingleAsync<int>(addLand4);
                int landId5 = await db.QuerySingleAsync<int>(addLand5);
                int landId6 = await db.QuerySingleAsync<int>(addLand6);

                string addUserLand1 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId1}, {userId1}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand2 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId2}, {userId1}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand3 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId3}, {userId1}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand4 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId4}, {userId1}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand5 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId5}, {userId1}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand6 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId6}, {userId1}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand7 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId1}, {userId2}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand8 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId2}, {userId2}, 3, 0, 0); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand9 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId3}, {userId2}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addUserLand10 = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId4}, {userId2}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId1 = await db.QuerySingleAsync<int>(addUserLand1);
                int userLandId2 = await db.QuerySingleAsync<int>(addUserLand2);
                int userLandId3 = await db.QuerySingleAsync<int>(addUserLand3);
                int userLandId4 = await db.QuerySingleAsync<int>(addUserLand4);
                int userLandId5 = await db.QuerySingleAsync<int>(addUserLand5);
                int userLandId6 = await db.QuerySingleAsync<int>(addUserLand6);
                int userLandId7 = await db.QuerySingleAsync<int>(addUserLand7);
                int userLandId8 = await db.QuerySingleAsync<int>(addUserLand8);
                int userLandId9 = await db.QuerySingleAsync<int>(addUserLand9);
                int userLandId10 = await db.QuerySingleAsync<int>(addUserLand10);

                List<LandPositionDto> result = (await userLandRepository.GetUserLandsFromUserPerspectiveAsync(userId1)).ToList();

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

                result.Should().HaveCount(4);
                result[0].PositionX.Should().Be(1);
                result[0].PositionY.Should().Be(1);
                result[1].PositionX.Should().Be(1);
                result[1].PositionY.Should().Be(2);
                result[2].PositionX.Should().Be(1);
                result[2].PositionY.Should().Be(5);
                result[3].PositionX.Should().Be(1);
                result[3].PositionY.Should().Be(6);
                userResult.Should().HaveCount(0);
                landResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                userLandResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task GetUserLand_WhenUserLandExist_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test2@email.com', '123jlk1j22'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 6, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                string addUserLand = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId}, {userId}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId = await db.QuerySingleAsync<int>(addUserLand);

                UserLandDto userLand = await userLandRepository.GetUserLandAsync(userId, landId);

                userLand.Should().NotBeNull();
            }
        }

        [Test]
        public async Task UpdateUserLandStatus_WhenUserLandExist_ThenStatusShouldBeUpdated_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test2@email.com', '123jlk1j22'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 6, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                string addUserLand = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId}, {userId}, 3, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userLandId = await db.QuerySingleAsync<int>(addUserLand);

                await userLandRepository.UpdateUserLandStatusAsync(userLandId, LandsStatus.None);

                string getUserLand = $"select * from UserLand where UserId = {userId} and LandId = {landId}";
                UserLandDto userLandDto = await db.QuerySingleAsync<UserLandDto>(getUserLand);

                userLandDto.Should().NotBeNull();
                userLandDto.Status.Should().Be(LandsStatus.None);
            }
        }

        [Test]
        public async Task AddUserLand_WhenUserLandAdded_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test2@email.com', '123jlk1j22'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 6, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                await userLandRepository.AddUserLandAsync(new UserLandDto()
                {
                    LandId = landId,
                    Status = LandsStatus.Explored,
                    UserId = userId,
                });

                UserLandDto userLand = await userLandRepository.GetUserLandAsync(userId, landId);

                userLand.Should().NotBeNull();
                userLand.Status.Should().Be(LandsStatus.Explored);
            }
        }
    }
}
