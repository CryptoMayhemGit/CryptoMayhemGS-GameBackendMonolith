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
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest
{
    internal class DiscoveryMissionRepositoryTests : BaseRepositoryTests
    {
        private IDiscoveryMissionRepository discoveryMissionRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            discoveryMissionRepository = GetDiscoveryMissionRepository();
        }

        [Test]
        public async Task GetFinishedMissions_WhenMissionsExist_ThenGetThem_Test()
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

                string addMission1 = $"insert into mission.DiscoveryMission (NpcId, LandId, UserId, FinishDate) values ({npcId1},{landId1},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addMission2 = $"insert into mission.DiscoveryMission (NpcId, LandId, UserId, FinishDate) values ({npcId2},{landId2},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                await db.QuerySingleAsync<int>(addMission1);
                await db.QuerySingleAsync<int>(addMission2);

                IEnumerable<DiscoveryMissionDto> missions = await discoveryMissionRepository.GetFinishedMissionsAsync();

                missions.Should().HaveCount(2);
            }
        }

        [Test]
        public async Task RemoveMission_WhenMissionsExist_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addUser = "insert into dbo.GameUser (Email, WalletAddress) values ('test@email.com', '123jlk1j2'); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = await db.QuerySingleAsync<int>(addUser);

                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);

                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);

                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name1', '123kfg', null, 1, 1, 1, null, 1, {landId}, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);

                string addMission = $"insert into mission.DiscoveryMission (NpcId, LandId, UserId, FinishDate) values ({npcId},{landId},{userId},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int missionId = await db.QuerySingleAsync<int>(addMission);

                await discoveryMissionRepository.RemoveMissionAsync(missionId);

                string getMission = "select * from mission.DiscoveryMission";
                IEnumerable<dynamic> missions = await db.QueryAsync(getMission);

                missions.Should().HaveCount(0);
            }
        }
    }
}
