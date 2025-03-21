using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.ChangeGuildOwner;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class ChangeGuildOwnerCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildRepository guildRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildRepository = GetService<IGuildRepository>();
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            int userId = 18302;

            ChangeGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ChangeGuildOwnerCommandRequest()
            {
                NewOwnerId = user.Entity.Id,
                UserId = userId
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {userId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerNotHaveGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ChangeGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ChangeGuildOwnerCommandRequest()
            {
                NewOwnerId = user.Entity.Id,
                UserId = owner.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User doesn't have guild.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenUserIsNotOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            ChangeGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ChangeGuildOwnerCommandRequest()
            {
                NewOwnerId = owner.Entity.Id,
                UserId = user.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User isn't guild owner.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenNewOwnerNotBelongsToGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            ChangeGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ChangeGuildOwnerCommandRequest()
            {
                NewOwnerId = user.Entity.Id,
                UserId = owner.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"New owner doesn't belong to the guild.");
            result.Errors.First().PropertyName.Should().Be($"NewOwnerId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenNewOwnerIsOldOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            ChangeGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ChangeGuildOwnerCommandRequest()
            {
                NewOwnerId = owner.Entity.Id,
                UserId = owner.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Owner cannot change owner to himself.");
            result.Errors.First().PropertyName.Should().Be($"NewOwnerId");

        }
    }
}
