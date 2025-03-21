using FluentValidation;
using Mayhen.Bl.Commands.GetLandDetails;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetLandDetailsCommandRequestValidator : BaseValidator<GetLandDetailsCommandRequest>
    {
        public GetLandDetailsCommandRequestValidator()
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
