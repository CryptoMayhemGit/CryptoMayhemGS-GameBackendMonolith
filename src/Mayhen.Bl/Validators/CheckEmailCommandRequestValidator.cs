using FluentValidation;
using Mayhem.Messages;
using Mayhen.Bl.Commands.CheckEmail;
using Mayhen.Bl.Validators.Base;
using Mayhen.Bl.Validators.Helpers;

namespace Mayhen.Bl.Validators
{
    public class CheckEmailCommandRequestValidator : BaseValidator<CheckEmailCommandRequest>
    {
        public CheckEmailCommandRequestValidator()
        {
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(320)
                .EmailAddressValidator().WithMessage(BaseMessages.ValidEmailIsRequiredBaseMessage);
        }
    }
}
