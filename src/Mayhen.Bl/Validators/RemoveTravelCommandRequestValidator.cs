using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.RemoveTravel;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class RemoveTravelCommandRequestValidator : BaseValidator<RemoveTravelCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public RemoveTravelCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyNpc();
            VerifyTravel();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.NpcId).GreaterThanOrEqualTo(1);
        }

        private void VerifyNpc()
        {
            RuleFor(x => new { x.NpcId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Npc npc = await mayhemDataContext
                    .Npcs
                    .SingleOrDefaultAsync(x => x.Id == request.NpcId && x.UserId == request.UserId, cancellationToken: cancellation);

                if (npc == null)
                {
                    context.AddFailure(FailureMessages.NpcWithIdDoesNotExistFailure(request.NpcId));
                    return;
                }
            });
        }

        private void VerifyTravel()
        {
            RuleFor(x => new { x.NpcId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                bool existingTravel = await mayhemDataContext.Travels.Where(x => x.NpcId == request.NpcId).AnyAsync(cancellation);

                if (!existingTravel)
                {
                    context.AddFailure(FailureMessages.TravelWithNpcWithIdNotExist(request.NpcId));
                }
            });
        }
    }
}
