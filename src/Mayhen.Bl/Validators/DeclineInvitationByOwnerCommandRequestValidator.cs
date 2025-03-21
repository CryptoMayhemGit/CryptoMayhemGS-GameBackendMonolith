using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.DeclineInvitationByOwner;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class DeclineInvitationByOwnerCommandRequestValidator : BaseValidator<DeclineInvitationByOwnerCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public DeclineInvitationByOwnerCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyGuildInvitation();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.InvitationId).GreaterThanOrEqualTo(1);
        }

        private void VerifyGuildInvitation()
        {
            RuleFor(x => new { x.InvitationId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                GuildInvitation invitation = await mayhemDataContext
                    .GuildInvitations
                    .Include(x => x.Guild)
                    .SingleOrDefaultAsync(x => x.Id == request.InvitationId && x.InvitationType == GuildInvitationsType.UserSentInvitation, cancellationToken: cancellation);

                if (invitation == null)
                {
                    context.AddFailure(FailureMessages.InvitationWithIdDoesNotExistFailure(request.InvitationId));
                    return;
                }
                if (invitation.Guild.OwnerId != request.UserId)
                {
                    context.AddFailure(FailureMessages.OnlyGuildOwnerCanDeclineInvitationFailure());
                }
            });
        }
    }
}
