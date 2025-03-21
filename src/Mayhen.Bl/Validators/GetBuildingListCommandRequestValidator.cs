using FluentValidation;
using Mayhen.Bl.Commands.GetBuildingList;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetBuildingListCommandRequestValidator : BaseValidator<GetBuildingListCommandRequest>
    {
        public GetBuildingListCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.LandTypeId).IsInEnum();
        }
    }
}
