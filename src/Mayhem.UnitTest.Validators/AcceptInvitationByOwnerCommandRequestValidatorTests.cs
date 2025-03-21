using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AcceptInvitationByOwner;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AcceptInvitationByOwnerCommandRequestValidatorTests : UnitTestBase
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
        public async Task AcceptInvitationByOwner_WhenInvitationIsAcceptedByNotOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user.Entity.Id);

            AcceptInvitationByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AcceptInvitationByOwnerCommandRequest()
            {
                InvitationId = invitation.Invitation.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Only guild owner can accept the invitation.");
            result.Errors.First().PropertyName.Should().Be("InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByOwner_WhenInvitationNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            int invitationId = 50375;

            AcceptInvitationByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AcceptInvitationByOwnerCommandRequest()
            {
                InvitationId = invitationId,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be("InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            int invitationId = 50375;

            AcceptInvitationByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AcceptInvitationByOwnerCommandRequest()
            {
                InvitationId = invitationId,
                UserId = owner.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be("InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationIsAcceptedByNotInvitedUser_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);

            AcceptInvitationByOwnerCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AcceptInvitationByOwnerCommandRequest()
            {
                InvitationId = invitation.Invitation.Id,
                UserId = 12314122,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Invitation with id {invitation.Invitation.Id} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"InvitationId");
        }
    }
}
