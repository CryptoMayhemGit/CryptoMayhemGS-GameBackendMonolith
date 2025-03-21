using FluentValidation;
using Mayhen.Bl.Commands.GetGuildById;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetGuildByIdCommandRequestValidator : BaseValidator<GetGuildByIdCommandRequest>
    {
        public GetGuildByIdCommandRequestValidator()
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
