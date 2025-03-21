using FluentValidation;
using Mayhen.Bl.Commands.GetGuilds;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetGuildsCommandRequestValidator : BaseValidator<GetGuildsCommandRequest>
    {
        public GetGuildsCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.Limit).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Skip).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }
}
