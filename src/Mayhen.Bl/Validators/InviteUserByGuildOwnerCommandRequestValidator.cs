using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.InviteUserByGuildOwner;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Mayhen.Bl.Validators
{
    public class InviteUserByGuildOwnerCommandRequestValidator : BaseValidator<InviteUserByGuildOwnerCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public InviteUserByGuildOwnerCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyUser();
            VerifInvitedUser();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.InvitedUserId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.UserId).NotEqual(x => x.InvitedUserId).WithMessage(BaseMessages.UserCannotInviteHimselfBaseMessage);
        }

        private void VerifyUser()
        {
            RuleFor(x => new { x.UserId, x.InvitedUserId }).CustomAsync(async (request, context, cancellation) =>
             {
                 Guild guild = await mayhemDataContext
                     .Guilds
                     .SingleOrDefaultAsync(x => x.OwnerId == request.UserId, cancellationToken: cancellation);

                 if (guild == null)
                 {
                     context.AddFailure(FailureMessages.UserDoesNotHaveGuildFailure());
                     return;
                 }

                 await VerifyInvitationWasSentEarlier(context, guild.Id, request.InvitedUserId);
             });
        }

        private async Task VerifyInvitationWasSentEarlier(ValidationContext<InviteUserByGuildOwnerCommandRequest> context, int guildId, int invitedUserId)
        {
            GuildInvitation existingInvitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.UserId == invitedUserId && x.GuildId == guildId);

            if (existingInvitation?.InvitationType == GuildInvitationsType.UserBeenInvited)
            {
                context.AddFailure(FailureMessages.UserHasAlreadyBeenInvitedFailure());
            }
        }

        private void VerifInvitedUser()
        {
            RuleFor(x => x.InvitedUserId).CustomAsync(async (invitedUserId, context, cancellation) =>
            {
                GameUser invitedUser = await mayhemDataContext
                    .GameUsers
                    .SingleOrDefaultAsync(x => x.Id == invitedUserId, cancellationToken: cancellation);

                if (invitedUser == null)
                {
                    context.AddFailure(FailureMessages.InvitedUserDoesNotExistFailure());
                }
                else if (invitedUser.GuildId != null)
                {
                    context.AddFailure(FailureMessages.InvitedUserIsAlreadyInGuildFailure());
                }
            });
        }
    }
}
