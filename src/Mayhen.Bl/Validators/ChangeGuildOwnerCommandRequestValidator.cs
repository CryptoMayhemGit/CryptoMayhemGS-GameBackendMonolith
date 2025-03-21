using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.ChangeGuildOwner;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class ChangeGuildOwnerCommandRequestValidator : BaseValidator<ChangeGuildOwnerCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public ChangeGuildOwnerCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyGameUser();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.NewOwnerId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.NewOwnerId).NotEqual(x => x.UserId).WithMessage(BaseMessages.OwnerCannotChangeOwnerToHimselfBaseMessage);
        }

        private void VerifyGameUser()
        {
            RuleFor(x => new { x.NewOwnerId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                GameUser user = await mayhemDataContext
                   .GameUsers
                   .Include(x => x.GuildOwner)
                   .ThenInclude(x => x.Users)
                   .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellation);

                if (user == null)
                {
                    context.AddFailure(FailureMessages.UserWithIdDoesNotExistFailure(request.UserId));
                }
                else if (user.GuildId == null)
                {
                    context.AddFailure(FailureMessages.UserDoesNotHaveGuildFailure());
                }
                else if (user.GuildOwner == null)
                {
                    context.AddFailure(FailureMessages.UserIsNotGuildOwnerFailure());
                }
                else if (!user.GuildOwner.Users.Select(x => x.Id).Contains(request.NewOwnerId))
                {
                    context.AddFailure(FailureMessages.NewOwnerDoesNotBelongToGuildFailure());
                }
            });
        }
    }
}
