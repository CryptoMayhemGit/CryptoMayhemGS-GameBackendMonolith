using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.ChangeNpcHealthState;
using Mayhen.Bl.Commands.GetAvailableNpcs;
using Mayhen.Bl.Commands.MoveNpc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Npc)]
    public class NpcController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public NpcController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the avaliable npcs by user identifier.
        /// </summary>
        /// <returns></returns>
        [Route("Avaliable")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAvailableNpcsCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvaliableNpcs()
        {
            GetAvailableNpcsCommandResponse response = await mediator.Send(new GetAvailableNpcsCommandRequest(UserId));

            return Ok(response);
        }

        /// <summary>
        /// Changes the state of the NPC health.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("HealthState")]
        [HttpPut]
        [ProducesResponseType(typeof(MoveNpcCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeNpcHealthState([FromBody] ChangeNpcHealthStateCommandRequest request)
        {
            request.UserId = UserId;
            ChangeNpcHealthStateCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }
    }
}