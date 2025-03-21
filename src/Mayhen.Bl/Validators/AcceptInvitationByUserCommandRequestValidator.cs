using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.AcceptInvitationByUser;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhen.Bl.Validators
{
    public class AcceptInvitationByUserCommandRequestValidator : BaseValidator<AcceptInvitationByUserCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public AcceptInvitationByUserCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyUserInvitation();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.InvitationId).GreaterThanOrEqualTo(1);
        }

        public async Task VerifyGuildAndUser(ValidationContext<AcceptInvitationByUserCommandRequest> context, int invitationId)
        {
            GuildInvitation invitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.Id == invitationId);

            Guild guild = await mayhemDataContext
                .Guilds
                .Include(x => x.Users)
                .Include(x => x.GuildBuildings)
                .ThenInclude(x => x.GuildBuildingBonuses)
                .SingleOrDefaultAsync(x => x.Id == invitation.GuildId);

            if (guild == null)
            {
                context.AddFailure(FailureMessages.GuildWithIdDoesNotExistFailure(guild.Id));
            }

            GameUser user = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == invitation.UserId);

            if (user == null)
            {
                context.AddFailure(FailureMessages.UserWithIdDoesNotExistFailure(invitation.UserId));
            }

            if (guild.Users.Select(x => x.Id).Contains(invitation.UserId))
            {
                context.AddFailure(FailureMessages.UserWithIdIsAlreadyInThisGuildFailure(invitation.UserId));
            }

            if (user.GuildId != null)
            {
                context.AddFailure(FailureMessages.UserWithIdIsAlreadyInOtherGuildFailure(invitation.UserId));
            }
        }

        private void VerifyUserInvitation()
        {
            RuleFor(x => new { x.InvitationId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                GuildInvitation invitation = await mayhemDataContext
                    .GuildInvitations
                    .SingleOrDefaultAsync(x => x.Id == request.InvitationId && x.InvitationType == GuildInvitationsType.UserBeenInvited, cancellationToken: cancellation);

                if (invitation == null)
                {
                    context.AddFailure(FailureMessages.InvitationWithIdDoesNotExistFailure(request.InvitationId));

                }
                else if (invitation.UserId != request.UserId)
                {
                    context.AddFailure(FailureMessages.OnlyUserFromInvitationCanAcceptFailure());
                }
                else
                {
                    await VerifyGuildAndUser(context, request.InvitationId);
                }
            });
        }
    }
}
