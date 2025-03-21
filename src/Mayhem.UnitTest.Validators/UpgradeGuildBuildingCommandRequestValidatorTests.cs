using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.UpgradeGuildBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class UpgradeGuildBuildingCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildBuildingRepository guildBuildingRepository;
        private ICostsValidationService costsValidationService;
        private IGuildImprovementValidationService guildImprovementValidationService;
        private IImprovementRepository improvementRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildBuildingRepository = GetService<IGuildBuildingRepository>();
            costsValidationService = GetService<ICostsValidationService>();
            guildImprovementValidationService = GetService<IGuildImprovementValidationService>();
            improvementRepository = GetService<IImprovementRepository>();
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenGuildBuildingNotExist_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;

            int buildingId = 3451;
            UpgradeGuildBuildingCommandRequestValidator validator = new(mayhemDataContext, guildBuildingRepository, costsValidationService, guildImprovementValidationService, improvementRepository);
            ValidationResult result = validator.Validate(new UpgradeGuildBuildingCommandRequest()
            {
                UserId = userId,
                GuildBuildingId = buildingId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild building with id {buildingId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"BuildingId");
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenUserIsNotOwner_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingDto guildBuilding = await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, GuildBuildingsType.MechBoard, userId);
            UpgradeGuildBuildingCommandRequestValidator validator = new(mayhemDataContext, guildBuildingRepository, costsValidationService, guildImprovementValidationService, improvementRepository);
            ValidationResult result = validator.Validate(new UpgradeGuildBuildingCommandRequest()
            {
                UserId = user.Entity.Id,
                GuildBuildingId = guildBuilding.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Guild doesn't exist.");
            result.Errors.First().PropertyName.Should().Be("Guild");
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenUserDoesntHaveResources_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingDto building = await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, GuildBuildingsType.MechBoard, userId);
            foreach (GuildResource resource in guild.Entity.GuildResources)
            {
                resource.Value = 0;
            }
            await mayhemDataContext.SaveChangesAsync();

            UpgradeGuildBuildingCommandRequestValidator validator = new(mayhemDataContext, guildBuildingRepository, costsValidationService, guildImprovementValidationService, improvementRepository);
            ValidationResult result = validator.Validate(new UpgradeGuildBuildingCommandRequest()
            {
                UserId = userId,
                GuildBuildingId = building.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Guild doesn't have enough resource.");
            result.Errors.First().PropertyName.Should().Be("Guild");
        }
    }
}
