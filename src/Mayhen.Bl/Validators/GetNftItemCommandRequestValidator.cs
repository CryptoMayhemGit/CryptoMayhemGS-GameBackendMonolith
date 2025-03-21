using FluentValidation;
using Mayhen.Bl.Commands.GetNftItem;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetNftItemCommandRequestValidator : BaseValidator<GetNftItemCommandRequest>
    {
        public GetNftItemCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.ItemNftId).GreaterThanOrEqualTo(1);
        }
    }
}
