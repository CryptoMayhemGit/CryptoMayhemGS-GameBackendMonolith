using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AddGuildBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AddGuildBuildingCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildBuildingRepository guildBuildingRepository;
        private ICostsValidationService costsValidationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildBuildingRepository = GetService<IGuildBuildingRepository>();
            costsValidationService = GetService<ICostsValidationService>();
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildBuildingAlredyExist_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingsType guildBuildingsType = GuildBuildingsType.FightBoard;
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, guildBuildingsType, userId);

            AddGuildBuildingCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildBuildingCommandRequest()
            {
                GuildBuildingTypeId = guildBuildingsType,
                UserId = userId,
                GuildId = guild.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Building with type {guildBuildingsType} already exists.");
            result.Errors.First().PropertyName.Should().Be($"BuildingId");
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildDoesNotExist_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            int guildId = 1234;

            AddGuildBuildingCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildBuildingCommandRequest()
            {
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
                UserId = userId,
                GuildId = guildId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild with id {guildId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"GuildId");
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildDoesNotResources_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(0),
            });
            await mayhemDataContext.SaveChangesAsync();

            AddGuildBuildingCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildBuildingCommandRequest()
            {
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
                UserId = userId,
                GuildId = guild.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild doesn't have enough resource.");
            result.Errors.First().PropertyName.Should().Be($"Guild");
        }

        [Test]
        public async Task AddGuildBuilding_WhenUserIsNotOwner_ThenThrowException_Test()
        {
            int userId1 = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            int userId2 = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId1,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            AddGuildBuildingCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildBuildingCommandRequest()
            {
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
                UserId = userId2,
                GuildId = guild.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Only owner can add building.");
            result.Errors.First().PropertyName.Should().Be($"OwnerId");
        }
    }
}
