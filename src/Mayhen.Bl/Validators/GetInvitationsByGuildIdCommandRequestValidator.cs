using FluentValidation;
using Mayhen.Bl.Commands.GetInvitationsByGuildId;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class GetInvitationsByGuildIdCommandRequestValidator : BaseValidator<GetInvitationsByGuildIdCommandRequest>
    {
        public GetInvitationsByGuildIdCommandRequestValidator()
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
