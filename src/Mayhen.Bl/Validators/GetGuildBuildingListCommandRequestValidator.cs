using FluentValidation;
using Mayhen.Bl.Commands.GetGuildBuildingList;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetGuildBuildingListCommandRequestValidator : BaseValidator<GetGuildBuildingListCommandRequest>
    {
        public GetGuildBuildingListCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.GuildId).GreaterThanOrEqualTo(1);
        }
    }
}
