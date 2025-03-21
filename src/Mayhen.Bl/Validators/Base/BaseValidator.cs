using FluentValidation;

namespace Mayhen.Bl.Validators.Base
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}
