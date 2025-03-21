using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.ReleaseItemFromNpc;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class ReleaseItemFromNpcCommandRequestValidator : BaseValidator<ReleaseItemFromNpcCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public ReleaseItemFromNpcCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyItems();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.ItemId).GreaterThanOrEqualTo(1);
        }

        private void VerifyItems()
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
            });
        }
    }
}
