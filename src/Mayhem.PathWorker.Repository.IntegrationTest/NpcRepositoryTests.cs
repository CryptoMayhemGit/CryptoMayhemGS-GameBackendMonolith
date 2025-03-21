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
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest
{
    internal class NpcRepositoryTests : BaseRepositoryTests
    {
        private INpcRepository npcRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            npcRepository = GetNpcRepository();
        }

        [Test]
        public async Task GetNpc_WhenNpcExist_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1,null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);

                NpcDto result = await npcRepository.GetNpcAsync(npcId);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                await db.QueryAsync(removeNpc);

                string getNpcs = $"select * from [nft].[Npc]";
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);

                result.Id.Should().Be(npcId);
                npcsResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateNpcLand_WhenLandUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId = await db.QuerySingleAsync<int>(addLand);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1,null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);

                await npcRepository.UpdateNpcLandAsync(npcId, landId);
                string getLand = $"select LandId from nft.npc where id = {npcId}";
                int result = await db.QueryFirstOrDefaultAsync<int>(getLand);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string removeLand = $"delete from [nft].[Land] where id = {landId}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(removeLand);
                await db.QueryAsync(removeLandInstance);

                string getNpcs = $"select * from [nft].[Npc]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                IEnumerable<dynamic> landResult = await db.QueryAsync(getLand);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);

                result.Should().Be(landId);
                npcsResult.Should().HaveCount(0);
                landResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateNpcStatus_WhenStatusUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1,null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);

                await npcRepository.UpdateNpcStatusAsync(npcId, NpcsStatus.OnDiscoveryMission);

                NpcDto npc = await db.QuerySingleAsync<NpcDto>($"select * from [nft].[Npc] where id = {npcId}");
                await db.QueryAsync($"delete from [nft].[Npc] where id = {npcId}");
                IEnumerable<dynamic> npcsResult = await db.QueryAsync("select * from [nft].[Npc]");

                npc.NpcStatusId.Should().Be((int)NpcsStatus.OnDiscoveryMission);
                npcsResult.Should().HaveCount(0);
            }
        }
    }
}
