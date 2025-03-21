using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class TravelRepositoryTests : UnitTestBase
    {
        private ITravelRepository travelRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            travelRepository = GetService<ITravelRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetTravelsFromByLandId_WhenTravelsExists_ThenGetThem_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Travels.AddAsync(new Travel() { LandFromId = newLand.Entity.Id });
            await mayhemDataContext.Travels.AddAsync(new Travel() { LandFromId = newLand.Entity.Id });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<TravelDto> travels = await travelRepository.GetTravelsFromByLandIdAsync(newLand.Entity.Id);

            travels.Should().NotBeNull();
            travels.Should().HaveCount(2);
        }

        [Test]
        public async Task GetTravelsToByLandId_WhenTravelsExists_ThenGetThem_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Travels.AddAsync(new Travel() { LandToId = newLand.Entity.Id });
            await mayhemDataContext.Travels.AddAsync(new Travel() { LandToId = newLand.Entity.Id });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<TravelDto> travels = await travelRepository.GetTravelsToByLandIdAsync(newLand.Entity.Id);

            travels.Should().NotBeNull();
            travels.Should().HaveCount(2);
        }

        [Test]
        public async Task AddTravel_WhenTravelAdded_ThenGetIt_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, LandId = landFrom.Entity.Id });

            await mayhemDataContext.SaveChangesAsync();

            List<TravelDto> travels = new()
            {
                new TravelDto()
                {
                    LandFromId = landFrom.Entity.Id,
                    LandToId = landTo.Entity.Id,
                    NpcId = npc.Entity.Id,
                }
            };

            IEnumerable<TravelDto> travel = await travelRepository.AddTravelsAsync(travels);

            travel.Should().HaveCount(1);
        }

        [Test]
        public async Task AddTravels_WhenTravelsAdded_ThenGetThem_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> land1 = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> land2 = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, LandId = landFrom.Entity.Id, NpcStatusId = NpcsStatus.None });

            await mayhemDataContext.SaveChangesAsync();

            List<TravelDto> travels = new()
            {
                new TravelDto()
                {
                    LandFromId = landFrom.Entity.Id,
                    LandToId = land1.Entity.Id,
                    NpcId = npc.Entity.Id,
                },
                new TravelDto()
                {
                    LandFromId = land1.Entity.Id,
                    LandToId = land2.Entity.Id,
                    NpcId = npc.Entity.Id,
                },
                new TravelDto()
                {
                    LandFromId = land2.Entity.Id,
                    LandToId = landTo.Entity.Id,
                    NpcId = npc.Entity.Id,
                }
            };

            IEnumerable<TravelDto> travel = await travelRepository.AddTravelsAsync(travels);
            Npc npcDb = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc.Entity.Id);

            npcDb.NpcStatusId.Should().Be(NpcsStatus.OnTravel);

            travel.Should().HaveCount(3);
        }

        [Test]
        public async Task RemoveTravels_WhenTravelsDeleted_ThenGetTrue_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> land1 = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> land2 = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, LandId = landFrom.Entity.Id, NpcStatusId = NpcsStatus.OnTravel });

            await mayhemDataContext.SaveChangesAsync();

            List<TravelDto> travels = new()
            {
                new TravelDto()
                {
                    LandFromId = landFrom.Entity.Id,
                    LandToId = land1.Entity.Id,
                    NpcId = npc.Entity.Id,
                },
                new TravelDto()
                {
                    LandFromId = land1.Entity.Id,
                    LandToId = land2.Entity.Id,
                    NpcId = npc.Entity.Id,
                },
                new TravelDto()
                {
                    LandFromId = land2.Entity.Id,
                    LandToId = landTo.Entity.Id,
                    NpcId = npc.Entity.Id,
                }
            };

            await travelRepository.AddTravelsAsync(travels);
            bool result = await travelRepository.RemoveTravelsByNpcIdAsync(npc.Entity.Id);
            Npc npcDb = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc.Entity.Id);

            result.Should().BeTrue();
            npcDb.NpcStatusId.Should().Be(NpcsStatus.None);
        }
    }
}
