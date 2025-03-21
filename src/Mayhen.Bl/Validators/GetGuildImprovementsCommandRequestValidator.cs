using FluentValidation;
using Mayhen.Bl.Commands.GetGuildImprovements;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetGuildImprovementsCommandRequestValidator : BaseValidator<GetGuildImprovementsCommandRequest>
    {
        public GetGuildImprovementsCommandRequestValidator()
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
