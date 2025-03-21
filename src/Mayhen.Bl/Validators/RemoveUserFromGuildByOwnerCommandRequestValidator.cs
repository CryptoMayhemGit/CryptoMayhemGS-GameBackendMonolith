using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhen.Bl.Commands.RemoveUserFromGuildByOwner;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class RemoveUserFromGuildByOwnerCommandRequestValidator : BaseValidator<RemoveUserFromGuildByOwnerCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public RemoveUserFromGuildByOwnerCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyOwner();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.RemovedUserId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.RemovedUserId).NotEqual(x => x.UserId).WithMessage(BaseMessages.OwnerCannotRemoveHimselfBaseMessage);
        }

        private void VerifyOwner()
        {
            RuleFor(x => new { x.RemovedUserId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Guild guild = await mayhemDataContext
                   .Guilds
                   .Include(x => x.Users)
                   .SingleOrDefaultAsync(x => x.OwnerId == request.UserId, cancellationToken: cancellation);

                if (guild == null)
                {
                    context.AddFailure(FailureMessages.UserDoesNotHaveGuildFailure());
                    return;
                }

                if (!guild.Users.Select(x => x.Id).Contains(request.RemovedUserId))
                {
                    context.AddFailure(FailureMessages.UserWithIdDoesNotBelongToGuildFailure(request.RemovedUserId, guild.Name));
                }
            });
        }
    }
}
