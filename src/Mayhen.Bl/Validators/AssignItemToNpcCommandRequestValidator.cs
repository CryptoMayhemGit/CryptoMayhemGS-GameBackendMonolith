using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.AssignItemToNpc;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class AssignItemToNpcCommandRequestValidator : BaseValidator<AssignItemToNpcCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public AssignItemToNpcCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyItems();
            VerifyNpc();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.ItemId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.NpcId).GreaterThanOrEqualTo(1);
        }

        private void VerifyNpc()
        {
            RuleFor(x => new { x.ItemId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Item item = await mayhemDataContext
                    .Items
                    .SingleOrDefaultAsync(x => x.Id == request.ItemId && x.UserId == request.UserId, cancellationToken: cancellation);

                if (item == null)
                {
                    context.AddFailure(FailureMessages.ItemWithIdDoesNotExistFailure(request.ItemId));
                }
                else if (item.Npc != null)
                {
                    context.AddFailure(FailureMessages.ItemWithIdIsAlreadyAssignedFailure(request.ItemId));
                }
                else if (!item.IsMinted)
                {
                    context.AddFailure(FailureMessages.ItemWithIdIsNotMintedFailure(request.ItemId));
                }
            });
        }

        private void VerifyItems()
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
                if (npc.ItemId != null)
                {
                    context.AddFailure(FailureMessages.NpcWithIdHasItemFailure(request.NpcId));
                }
            });
        }
    }
}
