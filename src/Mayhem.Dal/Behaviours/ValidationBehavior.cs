using FluentValidation;
using Mayhem.Util.Exceptions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Dal.Behaviours
{
    /// <summary>
    /// This behavior is responsible for validation requests for each mediator request    
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!validators.Any())
            {
                return await next();
            }

            ValidationContext<TRequest> context = new(request);
            List<ValidationMessage> errorsDictionary = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().First()
                    })
                .Select(x => new ValidationMessage(x.Key, x.Values)).ToList();

            if (errorsDictionary.Any())
            {
                throw new Util.Exceptions.ValidationException(errorsDictionary);
            }

            return await next();
        }
    }
}
