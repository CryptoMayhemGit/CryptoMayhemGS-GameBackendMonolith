using Dapper;
using FluentAssertions;
using Mayhem.PathWorker.Repository.IntegrationTest.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Path.Interfaces;
using Mayhem.Worker.Path.Services;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest.E2E
{
    internal class PathFindingIntegrationTests : BaseRepositoryTests
    {
        private IPathWorkerService pathWorkerService;
        private ILandRepository landRepository;
        private ITravelRepository travelRepository;

        private readonly List<int> ownedLandIds = new() { 1, 2, 9 };
        private readonly List<int> ownedBlackLandIds = new() { 10 };
        private readonly List<int> discoveredLandIds = new() { 3, 12, 17, 18, 20, 21, 27, 28, 29, 30, 37, 38, 39, 42, 43, 45, 46, 50, 51, 52, 58, 59 };
        private readonly List<int> landsWithWhiteFog = new() { 4, 5, 11, 13, 14, 19, 22, 23, 25, 26, 31, 33, 34, 35, 36, 40, 41, 44, 47, 49, 53, 54, 55 };


        [OneTimeSetUp]
        public void SetUp()
        {
            pathWorkerService = new PathWorkerService(
                GetTravelRepository(),
                GetLandRepository(),
                GetNpcRepository(),
                GetUserLandRepository(),
                GetPathFindingService(),
                MayhemConfigurationService,
                new Mock<ILogger<PathWorkerService>>().Object);

            landRepository = GetLandRepository();
            travelRepository = GetTravelRepository();
        }

        [Test]
        public async Task CreateStruture_MoveNpcFormLand20ToLand21Point_WhenNpcMoved_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 11, 14, 19, 23, 25, 26, 27, 31, 33, 34, 35, 36, 40, 41, 44, 47, 49, 53, 54, 55 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom = 20;
                long landTo = 21;
                await db.QueryAsync($"update nft.npc set LandId = {landTo} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_MoveNpcFormLand20ToLand1Point_WhenNpcMoved_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 11, 12, 13, 14, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 33, 34, 35, 36, 40, 41, 44, 47, 49, 53, 54, 55 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom = 20;
                long landTo = 1;
                await db.QueryAsync($"update nft.npc set LandId = {landTo} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_MoveNpcFormLand20ToLand34Point_WhenNpcMoved_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 11, 12, 13, 14, 19, 20, 21, 22, 23, 27, 28, 31, 36, 40, 44, 47, 49, 53, 54, 55 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom = 20;
                long landTo = 34;
                await db.QueryAsync($"update nft.npc set LandId = {landTo} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_MoveNpcFormLand20ToLand36AndNextToLand23Point_WhenNpcMoved_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 11, 12, 13, 19, 20, 21, 25, 26, 27, 28, 33, 34, 35, 36, 40, 41, 44, 47, 49, 53, 54, 55 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom = 20;
                long landTo1 = 36;
                long landTo2 = 23;
                await db.QueryAsync($"update nft.npc set LandId = {landTo1} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo1, userId);

                await db.QueryAsync($"update nft.npc set LandId = {landTo2} where id = 1");
                await landRepository.AddFogToLandsAsync(landTo1, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo2, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_MoveFewNpcs_WhenNpcsMoved_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 11, 12, 19, 20, 27, 28, 31, 38, 39, 40, 46, 47, 49, 50, 51, 54, 55, 58, 59 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom1 = 20;
                long landTo1 = 22;
                await db.QueryAsync($"update nft.npc set LandId = {landTo1} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom1, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo1, userId);

                long landFrom2 = 38;
                long landTo2 = 34;
                await db.QueryAsync($"update nft.npc set LandId = {landTo2} where id = 2");
                await landRepository.AddFogToLandsAsync(landFrom2, userId, 2);
                await landRepository.RemoveFogFromLandsAsync(landTo2, userId);

                long landFrom3 = 51;
                long landTo3 = 44;
                await db.QueryAsync($"update nft.npc set LandId = {landTo3} where id = 3");
                await landRepository.AddFogToLandsAsync(landFrom3, userId, 3);
                await landRepository.RemoveFogFromLandsAsync(landTo3, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_MoveNpcs_WhenNpcThereAndBack_ThenChangeWhiteFogForLands_Test()
        {
            List<int> expectedLandIds = new() { 4, 5, 13, 14, 22, 23, 25, 26, 31, 33, 34, 35, 36, 40, 41, 44, 47, 49, 53, 54, 55 };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                long landFrom1 = 20;
                long landTo1 = 21;
                await db.QueryAsync($"update nft.npc set LandId = {landTo1} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom1, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo1, userId);

                long landFrom2 = 21;
                long landTo2 = 20;
                await db.QueryAsync($"update nft.npc set LandId = {landTo2} where id = 1");
                await landRepository.AddFogToLandsAsync(landFrom2, userId, 1);
                await landRepository.RemoveFogFromLandsAsync(landTo2, userId);

                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_AddTravelAndStartWorker_WhenNpcMoved_ThenChangeWhiteFogForLands_Test()
        {
            int npcId = 1;
            List<TravelDto> tralves = new()
            {
                new TravelDto()
                {
                    LandFromId = 20,
                    LandToId = 28,
                    FinishDate = DateTime.UtcNow.AddSeconds(1),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 28,
                    LandToId = 36,
                    FinishDate = DateTime.UtcNow.AddSeconds(1),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 36,
                    LandToId = 35,
                    FinishDate = DateTime.UtcNow.AddSeconds(1),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 35,
                    LandToId = 34,
                    FinishDate = DateTime.UtcNow.AddSeconds(1),
                    NpcId = npcId,
                }
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int userId = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test@email.com', '2l3j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, userId);
                await CreateNpcAsync(db, userId);

                await travelRepository.AddTravelsAsync(tralves);

                await pathWorkerService.StartWorkAsync();

                List<int> expectedLandIds = new() { 4, 5, 11, 12, 13, 14, 19, 20, 21, 22, 23, 27, 28, 31, 36, 40, 44, 47, 49, 53, 54, 55 };
                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        [Test]
        public async Task CreateStruture_AddTravelAndStartWorker_WhenNpcEnemyExistOnRoad_ThenRecalculateTravel_Test()
        {
            int npcId = 1;
            List<TravelDto> tralves = new()
            {
                new TravelDto()
                {
                    LandFromId = 20,
                    LandToId = 28,
                    FinishDate = DateTime.UtcNow.AddSeconds(1),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 28,
                    LandToId = 36,
                    FinishDate = DateTime.UtcNow.AddSeconds(2),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 36,
                    LandToId = 35,
                    FinishDate = DateTime.UtcNow.AddSeconds(31),
                    NpcId = npcId,
                },
                new TravelDto()
                {
                    LandFromId = 35,
                    LandToId = 34,
                    FinishDate = DateTime.UtcNow.AddSeconds(32),
                    NpcId = npcId,
                }
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int landInstanceId = await db.QuerySingleAsync<int>("insert into dbo.LandInstance default values; SELECT CAST(SCOPE_IDENTITY() as int)");
                int user1Id = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test1@email.com', '2l31j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                int user2Id = await db.QuerySingleAsync<int>("insert into dbo.GameUser (email, walletaddress) values ('test2@email.com', '2l32j4l23j'); SELECT CAST(SCOPE_IDENTITY() as int)");
                await CreateLandsAsync(db, landInstanceId);
                await CreateUserLandsAsync(db, user1Id);
                await CreateEnemyUserLandsAsync(db, user2Id);
                await CreateNpcAsync(db, user1Id);
                await CreateEnemyNpcAsync(db, user2Id);

                await travelRepository.AddTravelsAsync(tralves);

                IEnumerable<dynamic> travels;
                do
                {
                    travels = await db.QueryAsync($"select * from [dbo].[travel] where NpcId = {npcId}");
                    await pathWorkerService.StartWorkAsync();
                }
                while (travels.Any());


                List<int> expectedLandIds = new() { 4, 5, 11, 12, 13, 14, 19, 20, 21, 22, 23, 27, 28, 31, 36, 40, 44, 47, 49, 53, 54, 55 };
                List<int> userLandResult = (await db.QueryAsync<int>("select LandId from userland where hasfog = 1 order by LandId")).ToList();
                for (int i = 0; i < expectedLandIds.Count; i++)
                {
                    expectedLandIds[i].Should().Be(userLandResult[i]);
                }
            }
        }

        private async Task CreateLandsAsync(IDbConnection db, int landInstanceId)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    string query = $"insert into [nft].[Land](LandInstanceId, LandTypeId, PositionX, PositionY, Name, Address, IsMinted) values ({landInstanceId}, 0, {x}, {y}, 'Land name', '{Guid.NewGuid()}', 1);";
                    await db.QueryAsync(query);
                }
            }
        }

        private async Task CreateUserLandsAsync(IDbConnection db, int userId)
        {
            for (int i = 0; i < ownedLandIds.Count; i++)
            {
                await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({ownedLandIds[i]}, {userId}, 3, 0, 1);");
            }

            for (int i = 0; i < ownedBlackLandIds.Count; i++)
            {
                await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({ownedBlackLandIds[i]}, {userId}, 1, 0, 1);");
            }

            for (int i = 0; i < discoveredLandIds.Count; i++)
            {
                await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({discoveredLandIds[i]}, {userId}, 3, 0, 0);");
            }

            for (int i = 0; i < landsWithWhiteFog.Count; i++)
            {
                await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values ({landsWithWhiteFog[i]}, {userId}, 3, 1, 0);");
            }
        }

        private async Task CreateEnemyUserLandsAsync(IDbConnection db, int userId)
        {
            await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values (36, {userId}, 3, 0, 0);");
            await db.QueryAsync($"insert into UserLand(LandId, UserId, Status, HasFog, Owned) values (27, {userId}, 3, 0, 0);");
        }

        private async Task CreateNpcAsync(IDbConnection db, int userId)
        {
            await db.QueryAsync($"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId}, 'Name', '123kfg', null, 1, 1, 1, null, 1, 20,1);");
            await db.QueryAsync($"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId}, 'Name', '123kfg', null, 1, 1, 1, null, 1, 38,1);");
            await db.QueryAsync($"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId}, 'Name', '123kfg', null, 1, 1, 1, null, 1, 51,1);");
        }

        private async Task CreateEnemyNpcAsync(IDbConnection db, int userId)
        {
            await db.QueryAsync($"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId}, 'Name', '123kfg', null, 1, 1, 1, null, 1, 36, 1);");
            await db.QueryAsync($"insert into nft.Npc (UserId, Name, Address, BuildingId, NpcTypeId, NpcHealthStateId, IsAvatar, ItemId, IsMinted, LandId, NpcStatusId) values ({userId}, 'Name', '123kfg', null, 1, 1, 1, null, 1, 27, 1);");
        }
    }
}
