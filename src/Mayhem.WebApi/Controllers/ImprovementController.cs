using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.AddGuildImprovement;
using Mayhen.Bl.Commands.AddImprovement;
using Mayhen.Bl.Commands.CheckGuildImprovement;
using Mayhen.Bl.Commands.CheckImprovement;
using Mayhen.Bl.Commands.GetGuildImprovements;
using Mayhen.Bl.Commands.GetImprovements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Improvement)]
    public class ImprovementController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public ImprovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Check if land can add an improvement.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Check")]
        [HttpGet]
        [ProducesResponseType(typeof(CheckImprovementCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckImprevement([FromQuery] CheckImprovementCommandRequest request)
        {
            CheckImprovementCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Check if guild can add an improvement.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Guild/Check")]
        [HttpGet]
        [ProducesResponseType(typeof(CheckGuildImprovementCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckGuildImprevement([FromQuery] CheckGuildImprovementCommandRequest request)
        {
            CheckGuildImprovementCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Add improvement to land.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AddImprovementCommandResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddImprovement([FromBody] AddImprovementCommandRequest request)
        {
            request.UserId = UserId;
            AddImprovementCommandResponse response = await mediator.Send(request);
            return CreatedAtAction(nameof(AddImprovement), response);
        }

        /// <summary>
        /// Add improvement to guild.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Guild")]
        [HttpPost]
        [ProducesResponseType(typeof(AddGuildImprovementCommandResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddGuildImprovement([FromBody] AddGuildImprovementCommandRequest request)
        {
            request.UserId = UserId;
            AddGuildImprovementCommandResponse response = await mediator.Send(request);
            return CreatedAtAction(nameof(AddGuildImprovement), response);
        }

        /// <summary>
        /// Get all improvements of the land.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        [Route("{landId:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetImprovementsCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImprovements([FromRoute] long landId)
        {
            GetImprovementsCommandResponse response = await mediator.Send(new GetImprovementsCommandRequest(landId));
            return Ok(response);
        }

        /// <summary>
        /// Get all improvements of the guild.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        [Route("Guild/{guildId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetGuildImprovementsCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGuildImprovements([FromRoute] int guildId)
        {
            GetGuildImprovementsCommandResponse response = await mediator.Send(new GetGuildImprovementsCommandRequest(guildId));
            return Ok(response);
        }
    }
}
