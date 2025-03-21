using Mayhem.Helper;
using Mayhem.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Dal.Behaviours
{
    /// <summary>
    /// This behavior is responsible for logging informatiom from each mediator request  
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation(LoggerMessages.HandlingCommand, request.GetGenericTypeName(), request);
            TResponse response = await next();
            logger.LogInformation(LoggerMessages.CommandHandled, request.GetGenericTypeName());
            logger.LogDebug(LoggerMessages.CommandHandledResponse, request.GetGenericTypeName(), response);

            return response;
        }
    }
}
