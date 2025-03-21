using Mayhem.Messages;
using Mayhem.Util.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.Dal.Filters
{
    /// <summary>
    /// Filter used in mvc to validate api requests    
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger<ValidationFilter> logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                KeyValuePair<string, IEnumerable<string>>[] errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                ErrorResponse errorResponse = new();

                foreach (KeyValuePair<string, IEnumerable<string>> error in errorsInModelState)
                {
                    foreach (string subError in error.Value)
                    {
                        errorResponse.Errors.Add(new ErrorModel(error.Key, subError));
                    }
                }

                logger.LogDebug(LoggerMessages.ValidationError(errorResponse.ToString()));
                context.Result = new BadRequestObjectResult(errorResponse);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return;
            }

            await next();
        }
    }
}
