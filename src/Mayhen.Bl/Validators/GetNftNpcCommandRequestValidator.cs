using FluentValidation;
using Mayhen.Bl.Commands.GetNftNpc;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetNftNpcCommandRequestValidator : BaseValidator<GetNftNpcCommandRequest>
    {
        public GetNftNpcCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.HeroNftId).GreaterThanOrEqualTo(1);
        }
    }
}
