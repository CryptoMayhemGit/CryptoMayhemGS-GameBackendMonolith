using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.LeaveGuild;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class LeaveGuildCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildRepository guildRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildRepository = GetService<IGuildRepository>();
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            int userId = 123431;
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            LeaveGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new LeaveGuildCommandRequest(userId));

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {userId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserInNotInGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            LeaveGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new LeaveGuildCommandRequest(user.Entity.Id));

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User doesn't have guild.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserIsOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            LeaveGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new LeaveGuildCommandRequest(owner.Entity.Id));

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User cannot leave the guild he owns.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }
    }
}
