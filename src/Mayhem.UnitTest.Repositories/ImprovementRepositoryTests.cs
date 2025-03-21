using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class ImprovementRepositoryTests : UnitTestBase
    {
        private IImprovementRepository improvementRepository;
        private IMayhemDataContext mayhemDataContext;
        private int userId;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            improvementRepository = GetService<IImprovementRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            await mayhemDataContext.Lands.AddAsync(new Land());

            userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            })).Entity.Id;

            await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
                OwnerId = userId,
            });

            await mayhemDataContext.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            List<Improvement> improvements = mayhemDataContext.Improvements.ToList();
            mayhemDataContext.Improvements.RemoveRange(improvements);
            List<GuildImprovement> guildImprovements = mayhemDataContext.GuildImprovements.ToList();
            mayhemDataContext.GuildImprovements.RemoveRange(guildImprovements);
            await mayhemDataContext.SaveChangesAsync();
        }

        [Test]
        public async Task AddImprovement_WhenImprovementAdded_ThenReturnTrue_Test()
        {
            ImprovementDto addedImprovement = await improvementRepository.AddImprovementAsync(new ImprovementDto()
            {
                ImprovementTypeId = ImprovementsType.DeepExcavationsTechnology,
                LandId = 1,
            }, userId);

            addedImprovement.Id.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public async Task AddGuildImprovement_WhenGuildImprovementAdded_ThenReturnTrue_Test()
        {
            GuildImprovementDto addedImprovement = await improvementRepository.AddGuildImprovementAsync(new GuildImprovementDto()
            {
                GuildImprovementTypeId = GuildImprovementsType.ImprovedFuelMixture,
                GuildId = 1,
            });

            addedImprovement.Id.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public async Task GetImprovementsByLandId_WhenImprovementsExists_ThenGetThem_Test()
        {
            await improvementRepository.AddImprovementAsync(new ImprovementDto()
            {
                ImprovementTypeId = ImprovementsType.BasicMiningCombine,
                LandId = 1,
            }, userId);

            await improvementRepository.AddImprovementAsync(new ImprovementDto()
            {
                ImprovementTypeId = ImprovementsType.ConstructionRobot,
                LandId = 1,
            }, userId);

            IEnumerable<ImprovementDto> improvements = await improvementRepository.GetImprovementsByLandIdAsync(1);

            improvements.Should().HaveCount(2);
        }

        [Test]
        public async Task GetGuildImprovementsByGuildId_WhenGuildImprovementsExists_ThenGetThem_Test()
        {
            await improvementRepository.AddGuildImprovementAsync(new GuildImprovementDto()
            {
                GuildImprovementTypeId = GuildImprovementsType.MolecularAnalysis,
                GuildId = 1,
            });

            await improvementRepository.AddGuildImprovementAsync(new GuildImprovementDto()
            {
                GuildImprovementTypeId = GuildImprovementsType.SupportPackage,
                GuildId = 1,
            });

            IEnumerable<GuildImprovementDto> improvements = await improvementRepository.GetGuildImprovementsByGuildIdAsync(1);

            improvements.Should().HaveCount(2);
        }

        [Test]
        public async Task GetImprovementByLandId_WhenImprovementsNotExists_ThenGetEmptyCollection_Test()
        {
            IEnumerable<ImprovementDto> improvements = await improvementRepository.GetImprovementsByLandIdAsync(1);
            improvements.Should().HaveCount(0);
        }

        [Test]
        public async Task GetGuildImprovementByLandId_WhenGuildImprovementsNotExists_ThenGetEmptyCollection_Test()
        {
            IEnumerable<GuildImprovementDto> improvements = await improvementRepository.GetGuildImprovementsByGuildIdAsync(100);
            improvements.Should().HaveCount(0);
        }
    }
}
