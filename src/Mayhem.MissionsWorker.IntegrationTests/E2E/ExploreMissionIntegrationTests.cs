using Dapper;
using FluentAssertions;
using Mayhem.Explore.Mission.Worker.Interfaces;
using Mayhem.Explore.Mission.Worker.Services;
using Mayhem.MissionsWorker.IntegrationTests.Base;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.MissionsWorker.IntegrationTests.E2E
{
    internal class ExploreMissionIntegrationTests : BaseRepositoryTests
    {
        private IExploreMissionService exploreMissionService;

        [OneTimeSetUp]
        public void SetUp()
        {
            exploreMissionService = new ExploreMissionService(
                GetExploreMissionRepository(),
                GetUserLandRepository(),
                GetNpcRepository(),
                new Mock<ILogger<ExploreMissionService>>().Object);
        }

        [Test]
        public async Task CreateStruture_AddMissions_ThenAddUserLandAndRemoveMissions_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test@email.com', '123jlk1j2'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);

                string addNpc1 = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name1', '123kfg', null, 1, 1, 1, null, 1, {landId1}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addNpc2 = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name2', '123kfg', null, 1, 1, 1, null, 1, {landId2}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId1 = await db.QuerySingleAsync<int>(addNpc1);
                int npcId2 = await db.QuerySingleAsync<int>(addNpc2);

                string addMission1 = $"insert into mission.ExploreMission (NpcId, LandId, UserId, FinishDate) values ({npcId1},{landId1},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addMission2 = $"insert into mission.ExploreMission (NpcId, LandId, UserId, FinishDate) values ({npcId2},{landId2},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                await db.QuerySingleAsync<int>(addMission1);
                await db.QuerySingleAsync<int>(addMission2);

                await exploreMissionService.StartWorkAsync();

                string getMissions = "select * from mission.ExploreMission";
                IEnumerable<dynamic> missions = await db.QueryAsync(getMissions);

                missions.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task CreateStruture_AddMissionsWithExistingUserLand_ThenAddUserLandAndRemoveMissions_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test@email.com', '123jlk1j2'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);

                string addUserLand = $"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landId1}, {userId}, 2, 0, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                await db.QuerySingleAsync<int>(addUserLand);

                string addNpc1 = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name1', '123kfg', null, 1, 1, 1, null, 1, {landId1}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addNpc2 = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name2', '123kfg', null, 1, 1, 1, null, 1, {landId2}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId1 = await db.QuerySingleAsync<int>(addNpc1);
                int npcId2 = await db.QuerySingleAsync<int>(addNpc2);

                string addMission1 = $"insert into mission.ExploreMission (NpcId, LandId, UserId, FinishDate) values ({npcId1},{landId1},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addMission2 = $"insert into mission.ExploreMission (NpcId, LandId, UserId, FinishDate) values ({npcId2},{landId2},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                await db.QuerySingleAsync<int>(addMission1);
                await db.QuerySingleAsync<int>(addMission2);

                await exploreMissionService.StartWorkAsync();

                string getMissions = "select * from mission.ExploreMission";
                IEnumerable<dynamic> missions = await db.QueryAsync(getMissions);

                missions.Should().HaveCount(0);
            }
        }
    }
}
