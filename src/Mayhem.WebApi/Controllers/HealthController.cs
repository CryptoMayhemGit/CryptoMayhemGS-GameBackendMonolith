using Mayhem.HealthCheck;
using Mayhem.WebApi.ActionNames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Route("api/" + ControllerNames.Health)]
    [ApiController]
    public class HealthController : HealthControllerBase
    {
        public HealthController(HealthCheckService healthCheckService) : base(healthCheckService) { }

        /// <summary>
        /// Check database connection - healthcheck.
        /// </summary>
        /// <returns></returns>
        [HttpGet(HealthCheckNames.Database)]
        public async Task<ActionResult> AgentDatabaseCrud()
        {
            return await CheckHealthAsync(HealthCheckNames.Database);
        }
    }
}
