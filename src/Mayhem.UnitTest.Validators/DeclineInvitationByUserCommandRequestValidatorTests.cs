using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.DeclineInvitationByUser;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class DeclineInvitationByUserCommandRequestValidatorTests : UnitTestBase
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
        public void DeclineInvitationByUser_WhenInvitationNotExist_ThenThrowException_Test()
        {
            int invitationId = 9381;

            DeclineInvitationByUserCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DeclineInvitationByUserCommandRequest()
            {
                InvitationId = invitationId,
                UserId = 12332,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task DeclineInvitationByUser_WhenInvitationIsDeclinedByNotOwner_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);

            DeclineInvitationByUserCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DeclineInvitationByUserCommandRequest()
            {
                InvitationId = invitation.Invitation.Id,
                UserId = 12332,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Only user from invitation can decline.");
            result.Errors.First().PropertyName.Should().Be($"InvitationId");
        }
    }
}
