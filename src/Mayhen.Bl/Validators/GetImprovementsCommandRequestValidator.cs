using FluentValidation;
using Mayhen.Bl.Commands.GetImprovements;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetImprovementsCommandRequestValidator : BaseValidator<GetImprovementsCommandRequest>
    {
        public GetImprovementsCommandRequestValidator()
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
