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
    internal class TravelRepositoryTests : BaseRepositoryTests
    {
        private ITravelRepository travelRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            travelRepository = GetTravelRepository();
        }

        [Test]
        public async Task GetTravels_WhenTravelExist_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1, null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);
                string addTravel = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId1},{landId2},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int travelId = await db.QuerySingleAsync<int>(addTravel);

                IEnumerable<TravelDto> result = await travelRepository.GetTravelsAsync();

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string remove1Land = $"delete from [nft].[Land] where id = {landId1}";
                string remove2Land = $"delete from [nft].[Land] where id = {landId2}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                string removeTravel = $"delete from [dbo].Travel where id = {travelId}";
                await db.QueryAsync(removeTravel);
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(remove1Land);
                await db.QueryAsync(remove2Land);
                await db.QueryAsync(removeLandInstance);

                string getLands = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getTravel = $"select * from [dbo].[Travel]";
                IEnumerable<dynamic> landsResult = await db.QueryAsync(getLands);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> travelResult = await db.QueryAsync(getTravel);

                result.Should().HaveCount(1);
                travelResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                landsResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task RemoveTravel_WhenTravelExist_ThenDeleteIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1, null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);
                string addTravel = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId1},{landId2},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int travelId = await db.QuerySingleAsync<int>(addTravel);

                await travelRepository.RemoveTravelAsync(travelId);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string remove1Land = $"delete from [nft].[Land] where id = {landId1}";
                string remove2Land = $"delete from [nft].[Land] where id = {landId2}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(remove1Land);
                await db.QueryAsync(remove2Land);
                await db.QueryAsync(removeLandInstance);

                string getLands = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getTravel = $"select * from [dbo].[Travel]";
                IEnumerable<dynamic> landsResult = await db.QueryAsync(getLands);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> travelResult = await db.QueryAsync(getTravel);

                travelResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                landsResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task RemoveTravelsByNpcId_WhenTravelsExists_ThenDeleteThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand3 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand4 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand5 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand6 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                int landId3 = await db.QuerySingleAsync<int>(addLand3);
                int landId4 = await db.QuerySingleAsync<int>(addLand4);
                int landId5 = await db.QuerySingleAsync<int>(addLand5);
                int landId6 = await db.QuerySingleAsync<int>(addLand6);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1, null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);
                string addTravel1 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId1},{landId2},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel2 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId2},{landId3},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel3 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId3},{landId4},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel4 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId4},{landId5},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel5 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId5},{landId6},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int travel1Id = await db.QuerySingleAsync<int>(addTravel1);
                int travel2Id = await db.QuerySingleAsync<int>(addTravel2);
                int travel3Id = await db.QuerySingleAsync<int>(addTravel3);
                int travel4Id = await db.QuerySingleAsync<int>(addTravel4);
                int travel5Id = await db.QuerySingleAsync<int>(addTravel5);

                await travelRepository.RemoveTravelsByNpcIdAsync(npcId);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string remove1Land = $"delete from [nft].[Land] where id = {landId1}";
                string remove2Land = $"delete from [nft].[Land] where id = {landId2}";
                string remove3Land = $"delete from [nft].[Land] where id = {landId3}";
                string remove4Land = $"delete from [nft].[Land] where id = {landId4}";
                string remove5Land = $"delete from [nft].[Land] where id = {landId5}";
                string remove6Land = $"delete from [nft].[Land] where id = {landId6}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(remove1Land);
                await db.QueryAsync(remove2Land);
                await db.QueryAsync(remove3Land);
                await db.QueryAsync(remove4Land);
                await db.QueryAsync(remove5Land);
                await db.QueryAsync(remove6Land);
                await db.QueryAsync(removeLandInstance);

                string getLands = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getTravel = $"select * from [dbo].[Travel]";
                IEnumerable<dynamic> landsResult = await db.QueryAsync(getLands);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> travelResult = await db.QueryAsync(getTravel);

                travelResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                landsResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task GetTravelsByNpcId_WhenTravelsExists_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand3 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand4 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand5 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand6 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                int landId3 = await db.QuerySingleAsync<int>(addLand3);
                int landId4 = await db.QuerySingleAsync<int>(addLand4);
                int landId5 = await db.QuerySingleAsync<int>(addLand5);
                int landId6 = await db.QuerySingleAsync<int>(addLand6);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1, null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);
                string addTravel1 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId1},{landId2},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel2 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId2},{landId3},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel3 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId3},{landId4},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel4 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId4},{landId5},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addTravel5 = $"insert into dbo.Travel (NpcId, LandFromId, LandToId, FinishDate) values ({npcId},{landId5},{landId6},GETUTCDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
                int travel1Id = await db.QuerySingleAsync<int>(addTravel1);
                int travel2Id = await db.QuerySingleAsync<int>(addTravel2);
                int travel3Id = await db.QuerySingleAsync<int>(addTravel3);
                int travel4Id = await db.QuerySingleAsync<int>(addTravel4);
                int travel5Id = await db.QuerySingleAsync<int>(addTravel5);

                IEnumerable<TravelDto> travels = await travelRepository.GetTravelsByNpcIdAsync(npcId);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string remove1Land = $"delete from [nft].[Land] where id = {landId1}";
                string remove2Land = $"delete from [nft].[Land] where id = {landId2}";
                string remove3Land = $"delete from [nft].[Land] where id = {landId3}";
                string remove4Land = $"delete from [nft].[Land] where id = {landId4}";
                string remove5Land = $"delete from [nft].[Land] where id = {landId5}";
                string remove6Land = $"delete from [nft].[Land] where id = {landId6}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                string removeTravels = $"delete from [dbo].[Travel] where NpcId = {npcId}";
                await db.QueryAsync(removeTravels);
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(remove1Land);
                await db.QueryAsync(remove2Land);
                await db.QueryAsync(remove3Land);
                await db.QueryAsync(remove4Land);
                await db.QueryAsync(remove5Land);
                await db.QueryAsync(remove6Land);
                await db.QueryAsync(removeLandInstance);

                string getLands = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getTravel = $"select * from [dbo].[Travel]";
                IEnumerable<dynamic> landsResult = await db.QueryAsync(getLands);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> travelResult = await db.QueryAsync(getTravel);

                travels.Should().HaveCount(5);
                landsResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                travelResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task AddTravels_WhenTravelsAdded_ThenGetThem_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addLandInstance = "insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)";
                int landInstanceId = await db.QuerySingleAsync<int>(addLandInstance);
                string addLand1 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand2 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand3 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 1, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand4 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 2, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand5 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 1, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                string addLand6 = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 1, 3, 2, 'Land name', '{Guid.NewGuid()}', 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int landId1 = await db.QuerySingleAsync<int>(addLand1);
                int landId2 = await db.QuerySingleAsync<int>(addLand2);
                int landId3 = await db.QuerySingleAsync<int>(addLand3);
                int landId4 = await db.QuerySingleAsync<int>(addLand4);
                int landId5 = await db.QuerySingleAsync<int>(addLand5);
                int landId6 = await db.QuerySingleAsync<int>(addLand6);
                string addNpc = $"insert into nft.Npc (Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ('Name', '123kfg', null, 1, 1, 1, null, 1, null, 1); SELECT CAST(SCOPE_IDENTITY() as int)";
                int npcId = await db.QuerySingleAsync<int>(addNpc);

                List<TravelDto> travelDtos = new()
                {
                    new TravelDto() { LandFromId = landId1, LandToId = landId2, NpcId = npcId, FinishDate = DateTime.UtcNow },
                    new TravelDto() { LandFromId = landId2, LandToId = landId3, NpcId = npcId, FinishDate = DateTime.UtcNow },
                    new TravelDto() { LandFromId = landId3, LandToId = landId4, NpcId = npcId, FinishDate = DateTime.UtcNow },
                    new TravelDto() { LandFromId = landId4, LandToId = landId5, NpcId = npcId, FinishDate = DateTime.UtcNow },
                    new TravelDto() { LandFromId = landId5, LandToId = landId6, NpcId = npcId, FinishDate = DateTime.UtcNow },
                };

                await travelRepository.AddTravelsAsync(travelDtos);
                IEnumerable<TravelDto> travels = await travelRepository.GetTravelsByNpcIdAsync(npcId);

                string removeNpc = $"delete from [nft].[Npc] where id = {npcId}";
                string remove1Land = $"delete from [nft].[Land] where id = {landId1}";
                string remove2Land = $"delete from [nft].[Land] where id = {landId2}";
                string remove3Land = $"delete from [nft].[Land] where id = {landId3}";
                string remove4Land = $"delete from [nft].[Land] where id = {landId4}";
                string remove5Land = $"delete from [nft].[Land] where id = {landId5}";
                string remove6Land = $"delete from [nft].[Land] where id = {landId6}";
                string removeLandInstance = $"delete from [dbo].[LandInstance] where id = {landInstanceId}";
                string removeTravels = $"delete from [dbo].[Travel] where NpcId = {npcId}";
                await db.QueryAsync(removeTravels);
                await db.QueryAsync(removeNpc);
                await db.QueryAsync(remove1Land);
                await db.QueryAsync(remove2Land);
                await db.QueryAsync(remove3Land);
                await db.QueryAsync(remove4Land);
                await db.QueryAsync(remove5Land);
                await db.QueryAsync(remove6Land);
                await db.QueryAsync(removeLandInstance);

                string getLands = $"select * from [nft].[Land]";
                string getLandInstance = $"select * from [dbo].[LandInstance] where id = {landInstanceId}";
                string getNpcs = $"select * from [nft].[Npc]";
                string getTravel = $"select * from [dbo].[Travel]";
                IEnumerable<dynamic> landsResult = await db.QueryAsync(getLands);
                IEnumerable<dynamic> landInstanceResult = await db.QueryAsync(getLandInstance);
                IEnumerable<dynamic> npcsResult = await db.QueryAsync(getNpcs);
                IEnumerable<dynamic> travelResult = await db.QueryAsync(getTravel);

                travels.Should().HaveCount(5);
                landsResult.Should().HaveCount(0);
                landInstanceResult.Should().HaveCount(0);
                npcsResult.Should().HaveCount(0);
                travelResult.Should().HaveCount(0);
            }
        }
    }
}
