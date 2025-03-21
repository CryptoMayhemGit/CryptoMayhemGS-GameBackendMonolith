using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class CreateGuildCommandRequestValidator : BaseValidator<CreateGuildCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public CreateGuildCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyGuildExistence();
            VerifyUserExistenceAndUserGuild();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(800);
        }

        private void VerifyGuildExistence()
        {
            RuleFor(x => x.Name).CustomAsync(async (name, context, cancellation) =>
            {
                Guild existingGuild = await mayhemDataContext
                    .Guilds
                    .SingleOrDefaultAsync(x => x.Name.ToUpper() == name.ToUpper(), cancellationToken: cancellation);

                if (existingGuild != null)
                {
                    context.AddFailure(FailureMessages.GuildWithNameAlreadyExistsFailure(name));
                }
            });
        }

        private void VerifyUserExistenceAndUserGuild()
        {
            RuleFor(x => x.UserId).CustomAsync(async (userId, context, cancellation) =>
            {
                GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellation);

                if (user == null)
                {
                    context.AddFailure(FailureMessages.UserWithIdDoesNotExistFailure(userId));
                    return;
                }
                if (user.GuildId != null)
                {
                    context.AddFailure(FailureMessages.UserWithIdAlreadyHasGuildFailure(userId));
                }
            });
        }
    }
}
