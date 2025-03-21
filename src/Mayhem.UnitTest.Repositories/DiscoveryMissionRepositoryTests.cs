using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    internal class DiscoveryMissionRepositoryTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IDiscoveryMissionRepository discoveryMissionRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            discoveryMissionRepository = GetService<IDiscoveryMissionRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task DiscoverMissionAsync_WhenMissionAdded_ThenGetMissionAndNewNpcStatus_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, LandId = land.Entity.Id, NpcStatusId = NpcsStatus.OnTravel });
            await mayhemDataContext.SaveChangesAsync();

            DiscoveryMissionDto mission = await discoveryMissionRepository.DiscoverMissionAsync(new DiscoveryMissionDto()
            {
                NpcId = npc.Entity.Id,
                LandId = land.Entity.Id,
                UserId = user.Entity.Id,
            });

            await mayhemDataContext.SaveChangesAsync();

            DiscoveryMission missionDb = await mayhemDataContext.DiscoveryMissions.SingleOrDefaultAsync(x => x.Id == mission.Id);
            Npc npcDb = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc.Entity.Id);

            missionDb.Should().NotBeNull();
            mission.Should().NotBeNull();
            npcDb.NpcStatusId.Should().Be(NpcsStatus.OnDiscoveryMission);
        }
    }
}
