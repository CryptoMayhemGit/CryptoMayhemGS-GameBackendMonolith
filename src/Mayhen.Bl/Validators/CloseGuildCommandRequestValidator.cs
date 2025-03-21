using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.CloseGuild;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class CloseGuildCommandRequestValidator : BaseValidator<CloseGuildCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public CloseGuildCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyGuildOwner();
        }

        private void VerifyGuildOwner()
        {
            RuleFor(x => x.UserId).CustomAsync(async (userId, context, cancellation) =>
            {
                Guild guild = await mayhemDataContext
                    .Guilds
                    .SingleOrDefaultAsync(x => x.OwnerId == userId, cancellationToken: cancellation);

                if (guild == null)
                {
                    context.AddFailure(FailureMessages.UserIsNotGuildOwnerFailure());
                }
            });
        }
    }
}
