using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.InviteUserByGuildOwner;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class InviteUserByGuildOwnerCommandRequestValidatorTests : UnitTestBase
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
        public async Task InviteUserByGuildOwner_WhenInvitedUserIsOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserByGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new InviteUserByGuildOwnerCommandRequest()
            {
                InvitedUserId = owner.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("UserId: User cannot invite himself.");
            result.Errors.First().PropertyName.Should().Be("UserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenOwnerNotOwnGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            InviteUserByGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new InviteUserByGuildOwnerCommandRequest()
            {
                InvitedUserId = user.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("User doesn't have guild.");
            result.Errors.First().PropertyName.Should().Be("UserId");
        }


        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserByGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new InviteUserByGuildOwnerCommandRequest()
            {
                InvitedUserId = 123123,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Invited user doesn't exist.");
            result.Errors.First().PropertyName.Should().Be("InvitedUserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserHasOtherGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            InviteUserByGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new InviteUserByGuildOwnerCommandRequest()
            {
                InvitedUserId = user.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Invited user is already in guild.");
            result.Errors.First().PropertyName.Should().Be("InvitedUserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserAlreadyReceivedInvitation_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);

            InviteUserByGuildOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new InviteUserByGuildOwnerCommandRequest()
            {
                InvitedUserId = user.Entity.Id,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("User has already been invited.");
            result.Errors.First().PropertyName.Should().Be("InvitedUserId");
        }
    }
}
