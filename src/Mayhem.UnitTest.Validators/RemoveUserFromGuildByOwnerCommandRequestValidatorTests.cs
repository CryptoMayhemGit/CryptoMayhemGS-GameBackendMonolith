using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.RemoveUserFromGuildByOwner;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class RemoveUserFromGuildByOwnerCommandRequestValidatorTests : UnitTestBase
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
        public async Task RemoveUserFromGuildByOwner_WhenUserIsNotOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            RemoveUserFromGuildByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new RemoveUserFromGuildByOwnerCommandRequest()
            {
                RemovedUserId = user.Entity.Id,
                UserId = 2340234,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("User doesn't have guild.");
            result.Errors.First().PropertyName.Should().Be("UserId");
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenUserNotBelongsToGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            RemoveUserFromGuildByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new RemoveUserFromGuildByOwnerCommandRequest()
            {
                RemovedUserId = user.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {user.Entity.Id} doesn't belong to the guild {guild.Name}.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenOwnerRemoveHimself_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            RemoveUserFromGuildByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new RemoveUserFromGuildByOwnerCommandRequest()
            {
                RemovedUserId = owner.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Owner cannot remove himself.");
            result.Errors.First().PropertyName.Should().Be($"RemovedUserId");
        }
    }
}
