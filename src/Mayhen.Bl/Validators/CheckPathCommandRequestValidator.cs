using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.CheckPath;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;

namespace Mayhen.Bl.Validators
{
    public class CheckPathCommandRequestValidator : BaseValidator<CheckPathCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;
        public CheckPathCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyNpcAndLands();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.LandFromId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.LandToId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.LandToId).NotEqual(x => x.LandFromId).WithMessage(BaseMessages.LandFromMustBeDifferentThanLandToBaseMessage);
        }

        private void VerifyNpcAndLands()
        {
            RuleFor(x => new { x.LandFromId, x.LandToId }).CustomAsync(async (request, context, cancellation) =>
            {
                Land landFrom = await mayhemDataContext
                    .Lands
                    .SingleOrDefaultAsync(x => x.Id == request.LandFromId, cancellationToken: cancellation);

                if (landFrom == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(request.LandFromId));
                    return;
                }

                Land landTo = await mayhemDataContext
                    .Lands
                    .SingleOrDefaultAsync(x => x.Id == request.LandToId, cancellationToken: cancellation);

                if (landTo == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(request.LandToId));
                    return;
                }
                if (landFrom.LandInstanceId != landTo.LandInstanceId)
                {
                    context.AddFailure(FailureMessages.LandFromAndLandToMustBelongToTheSameLandInstanceFailure());
                    return;
                }
            });
        }
    }
}
