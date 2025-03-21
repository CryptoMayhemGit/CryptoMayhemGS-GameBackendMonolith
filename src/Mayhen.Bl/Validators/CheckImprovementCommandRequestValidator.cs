using FluentValidation;
using Mayhen.Bl.Commands.CheckImprovement;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class CheckImprovementCommandRequestValidator : BaseValidator<CheckImprovementCommandRequest>
    {
        public CheckImprovementCommandRequestValidator()
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
            RuleFor(x => x.BuildingsTypeId).IsInEnum();
            RuleFor(x => x.Level).GreaterThanOrEqualTo(1);
        }
    }
}
