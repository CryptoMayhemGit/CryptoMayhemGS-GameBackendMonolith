using FluentValidation;
using Mayhen.Bl.Commands.GetInstance;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetLandInstanceCommandRequestValidator : BaseValidator<GetLandInstanceCommandRequest>
    {
        public GetLandInstanceCommandRequestValidator()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.InstanceId).GreaterThanOrEqualTo(1);
        }
    }
}
