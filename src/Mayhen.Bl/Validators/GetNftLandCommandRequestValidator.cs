using FluentValidation;
using Mayhen.Bl.Commands.GetNftLand;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetNftLandCommandRequestValidator : BaseValidator<GetNftLandCommandRequest>
    {
        public GetNftLandCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.LandNftId).GreaterThanOrEqualTo(1);
        }
    }
}
