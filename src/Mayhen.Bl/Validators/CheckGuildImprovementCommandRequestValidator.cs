using FluentValidation;
using Mayhen.Bl.Commands.CheckGuildImprovement;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class CheckGuildImprovementCommandRequestValidator : BaseValidator<CheckGuildImprovementCommandRequest>
    {
        public CheckGuildImprovementCommandRequestValidator()
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
            RuleFor(x => x.GuildBuildingsTypeId).IsInEnum();
            RuleFor(x => x.Level).GreaterThanOrEqualTo(1);
        }
    }
}
