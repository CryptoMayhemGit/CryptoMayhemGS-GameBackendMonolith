using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Services
{
    public class CostsValidationServiceTests : UnitTestBase
    {
        private ICostsValidationService costsValidationService;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            costsValidationService = GetService<ICostsValidationService>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task ValidateUser_WhenUserValid_ThenGetEmpty_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            Dictionary<ResourcesType, int> costs = ImprovementCostsDictionary.GetImprovementCosts(ImprovementsType.AdditionalTankForMechanium);

            List<ValidationMessage> result = await costsValidationService.ValidateUserAsync(costs, user.Entity.Id);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task ValidateUser_WhenUserNotValid_ThenGetMessages_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(0),
            });
            await mayhemDataContext.SaveChangesAsync();

            Dictionary<ResourcesType, int> costs = ImprovementCostsDictionary.GetImprovementCosts(ImprovementsType.AdditionalTankForMechanium);

            List<ValidationMessage> result = await costsValidationService.ValidateUserAsync(costs, user.Entity.Id);

            result.Should().HaveCount(3);
        }

        [Test]
        public async Task ValidateGuild_WhenGuildValid_ThenGetEmpty_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = user.Entity.Id,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            Dictionary<ResourcesType, int> costs = GuildImprovementCostsDictionary.GetGuildImprovementCosts(GuildImprovementsType.ImprovedTransmission);

            List<ValidationMessage> result = await costsValidationService.ValidateGuildAsync(costs, user.Entity.Id);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task ValidateGuild_WhenGuildNotValid_ThenGetMessages_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = user.Entity.Id,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(0),
            });
            await mayhemDataContext.SaveChangesAsync();

            Dictionary<ResourcesType, int> costs = GuildImprovementCostsDictionary.GetGuildImprovementCosts(GuildImprovementsType.ImprovedTransmission);

            List<ValidationMessage> result = await costsValidationService.ValidateGuildAsync(costs, user.Entity.Id);

            result.Should().HaveCount(4);
        }
    }
}
