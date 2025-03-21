using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Mayhen.Bl.Validators
{
    public class AskToJoinGuildByUserCommandRequestValidator : BaseValidator<AskToJoinGuildByUserCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public AskToJoinGuildByUserCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyUser();
            VerifyGuild();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.GuildId).GreaterThanOrEqualTo(1);
        }

        private void VerifyUser()
        {
            RuleFor(x => x.UserId).CustomAsync(async (userId, context, cancellation) =>
            {
                GameUser user = await mayhemDataContext
                    .GameUsers
                    .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellation);

                if (user == null)
                {
                    context.AddFailure(FailureMessages.UserWithIdDoesNotExistFailure(userId));
                }
                else if (user.GuildId != null)
                {
                    context.AddFailure(FailureMessages.UserWithIdIsAlreadyInOtherGuildFailure(userId));
                }
            });
        }

        private void VerifyGuild()
        {
            RuleFor(x => new { x.GuildId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Guild guild = await mayhemDataContext
                    .Guilds
                    .SingleOrDefaultAsync(x => x.Id == request.GuildId, cancellationToken: cancellation);

                if (guild == null)
                {
                    context.AddFailure(FailureMessages.GuildWithIdDoesNotExistFailure(request.GuildId));
                    return;
                }
                else if (guild.OwnerId == request.UserId)
                {
                    context.AddFailure(FailureMessages.UserCannotInviteHimselfFailure());
                    return;
                }

                await VerifyInvitationWasSentEarlier(context, (request.GuildId, request.UserId));
            });
        }

        private async Task VerifyInvitationWasSentEarlier(ValidationContext<AskToJoinGuildByUserCommandRequest> context, (int guildId, int userId) request)
        {
            GuildInvitation existingInvitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.UserId == request.userId && x.GuildId == request.guildId);

            if (existingInvitation?.InvitationType == GuildInvitationsType.UserSentInvitation)
            {
                context.AddFailure(FailureMessages.UserHasAlreadySentAnInvitationFailure());
            }
        }
    }
}
