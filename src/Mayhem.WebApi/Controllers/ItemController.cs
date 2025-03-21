using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.AssignItemToNpc;
using Mayhen.Bl.Commands.GetAvailableItems;
using Mayhen.Bl.Commands.GetUnavailableItems;
using Mayhen.Bl.Commands.ReleaseItemFromNpc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Item)]
    public class ItemController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public ItemController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get available items per user.
        /// </summary>
        /// <returns></returns>
        [Route("Available")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAvailableItemsCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailableItems()
        {
            GetAvailableItemsCommandResponse response = await mediator.Send(new GetAvailableItemsCommandRequest(UserId));

            return response == null ? NotFound() : Ok(response);
        }

        /// <summary>
        /// Get unavailable items per user.
        /// </summary>
        /// <returns></returns>
        [Route("Unavailable")]
        [HttpGet]
        [ProducesResponseType(typeof(GetUnavailableItemsCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUnavailableItems()
        {
            GetUnavailableItemsCommandResponse response = await mediator.Send(new GetUnavailableItemsCommandRequest(UserId));

            return response == null ? NotFound() : Ok(response);
        }

        /// <summary>
        /// Assigns the item to npc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Assign")]
        [HttpPut]
        [ProducesResponseType(typeof(AssignItemToNpcCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AssignItemToNpc([FromBody] AssignItemToNpcCommandRequest request)
        {
            request.UserId = UserId;
            AssignItemToNpcCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Releases the item from npc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Release")]
        [HttpPut]
        [ProducesResponseType(typeof(ReleaseItemFromNpcCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ReleaseItemFromNpc([FromBody] ReleaseItemFromNpcCommandRequest request)
        {
            request.UserId = UserId;
            ReleaseItemFromNpcCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }
    }
}
