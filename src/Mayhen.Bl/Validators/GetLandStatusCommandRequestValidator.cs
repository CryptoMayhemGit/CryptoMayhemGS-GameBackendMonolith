using FluentValidation;
using Mayhen.Bl.Commands.GetLandStatus;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetLandStatusCommandRequestValidator : BaseValidator<GetLandStatusCommandRequest>
    {
        public GetLandStatusCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.LandId).GreaterThanOrEqualTo(1);
        }
    }
}
