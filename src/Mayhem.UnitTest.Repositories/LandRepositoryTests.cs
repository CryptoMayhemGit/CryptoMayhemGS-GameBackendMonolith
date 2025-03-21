using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class LandRepositoryTests : UnitTestBase
    {
        private ILandRepository landRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            landRepository = GetService<ILandRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetLandById_WhenLandExists_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            LandDto land = await landRepository.GetLandByNftIdAsync(newLand.Entity.Id);

            land.Should().NotBeNull();
        }

        [Test]
        public async Task GetLandById_WhenLandNotExists_ThenGetNull_Test()
        {
            LandDto land = await landRepository.GetLandByNftIdAsync(1123);

            land.Should().BeNull();
        }

        [Test]
        public async Task AddLandsWithLands_WhenLandsAndLandsAdded_ThenGetThem_Test()
        {
            IEnumerable<LandDto> lands = await landRepository.AddLandsAsync(new List<LandDto>()
            {
                new LandDto(),
                new LandDto(),
            });


            lands.Should().NotBeNull();
            lands.Should().HaveCount(2);
            lands.Should().HaveCount(2);
        }

        [Test]
        public async Task GetSimpleLandsByInstanceId_WhenLandsExist_ThemGetThem_Test()
        {
            const int expectedLandsCount = 5;

            EntityEntry<LandInstance> newLandInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            for (int i = 0; i < expectedLandsCount; i++)
            {
                await mayhemDataContext.Lands.AddRangeAsync(new Land() { LandInstanceId = newLandInstance.Entity.Id });
            }
            await mayhemDataContext.Lands.AddRangeAsync(new Land(), new Land());

            await mayhemDataContext.SaveChangesAsync();
            IEnumerable<SimpleLandDto> lands = await landRepository.GetSimpleLandsByInstanceIdAsync(newLandInstance.Entity.Id);

            lands.Should().HaveCount(expectedLandsCount);
        }
    }
}
