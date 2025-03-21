using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Mayhem.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            string errorMessage = ex.Message;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                errorMessage = errorMessage + ". " + ex.Message;
            }

            if (context.Exception is NotFoundException nEx)
            {
                context.Result = new NotFoundObjectResult(nEx.ErrorResponse);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogDebug(errorMessage);
            }
            else if (context.Exception is ValidationException vEx)
            {
                context.Result = new BadRequestObjectResult(vEx.ErrorResponse);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogDebug(errorMessage);
            }
            else if (context.Exception is InternalException iEx)
            {
                context.Result = new ObjectResult(iEx.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(errorMessage);
            }
            else
            {
                context.Result = new ObjectResult(new ErrorResponse(new ErrorModel("InternalServerError", "Internal server error occured")));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(errorMessage);
            }

            context.ExceptionHandled = true;
        }
    }
}
