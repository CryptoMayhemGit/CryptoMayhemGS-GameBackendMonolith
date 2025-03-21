using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.CheckPath;
using Mayhen.Bl.Commands.MoveNpc;
using Mayhen.Bl.Commands.RemoveTravel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Travel)]
    public class TravelController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public TravelController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Move npc from land(A) to land(B).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Move")]
        [HttpPost]
        [ProducesResponseType(typeof(MoveNpcCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MoveNpc([FromBody] MoveNpcCommandRequest request)
        {
            request.UserId = UserId;
            MoveNpcCommandResponse response = await mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Check the path from land (A) to (B).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Check")]
        [HttpPost]
        [ProducesResponseType(typeof(CheckPathCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckPath([FromBody] CheckPathCommandRequest request)
        {
            request.UserId = UserId;
            CheckPathCommandResponse response = await mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Remove travels by npc id
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(CheckPathCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveTravel([FromBody] RemoveTravelCommandRequest request)
        {
            request.UserId = UserId;
            RemoveTravelCommandResponse response = await mediator.Send(request);

            return Ok(response);
        }
    }
}
