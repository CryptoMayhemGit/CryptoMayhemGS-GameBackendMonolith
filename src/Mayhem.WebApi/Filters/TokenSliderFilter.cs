using Mayhem.Messages;
using Mayhem.Util;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Filters
{
    public class TokenSliderFilter : IAsyncActionFilter
    {
        private readonly ILogger<TokenSliderFilter> logger;
        private readonly IAuthService web3AuthService;

        private readonly List<string> skippedList = new()
        {
            "AccountController",
            "HealthController"
        };

        public TokenSliderFilter(ILogger<TokenSliderFilter> logger, IAuthService web3AuthService)
        {
            this.logger = logger;
            this.web3AuthService = web3AuthService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            if (skippedList.Contains(context.Controller.GetType().Name))
            {
                return;
            }

            string token = context.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                int userId = JwtSecurityTokenHelper.GetUserId(token);
                string newToken = await web3AuthService.RefreshToken(userId);
                context.HttpContext.Response.Headers.Add("new-token", $"bearer {newToken}");

                logger.LogDebug(LoggerMessages.NewTokenGenerated);
            }
        }
    }
}
