using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.LeaveGuild;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class LeaveGuildCommandRequestValidator : BaseValidator<LeaveGuildCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public LeaveGuildCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyUser();
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
                    return;
                }

                if (user.GuildId == null)
                {
                    context.AddFailure(FailureMessages.UserDoesNotHaveGuildFailure());
                }

                if (user.GuildOwner != null)
                {
                    context.AddFailure(FailureMessages.UserCannotLeaveGuildHeOwnsFailure());
                }
            });
        }
    }
}
