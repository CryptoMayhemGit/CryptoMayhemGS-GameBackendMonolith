using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.CheckPurchaseLand;
using Mayhen.Bl.Commands.DiscoverLand;
using Mayhen.Bl.Commands.ExploreLand;
using Mayhen.Bl.Commands.GetInstance;
using Mayhen.Bl.Commands.GetLandDetails;
using Mayhen.Bl.Commands.GetLandStatus;
using Mayhen.Bl.Commands.GetUserLands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Land)]
    public class LandController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public LandController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get land details.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        [Route("{landId:long}/Details")]
        [HttpGet]
        [ProducesResponseType(typeof(GetLandDetailsCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLandDetails([FromRoute] long landId)
        {
            GetLandDetailsCommandResponse response = await mediator.Send(new GetLandDetailsCommandRequest(landId));

            return response == null ? NotFound() : Ok(response);
        }

        /// <summary>
        /// Get land status.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        [Route("{landId:long}/Status")]
        [HttpGet]
        [ProducesResponseType(typeof(GetLandStatusCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLandStatus([FromRoute] long landId)
        {
            GetLandStatusCommandResponse response = await mediator.Send(new GetLandStatusCommandRequest(landId));

            return Ok(response);
        }

        /// <summary>
        /// Get lands by instance id.
        /// </summary>
        /// <param name="instanceId">The land identifier.</param>
        /// <returns></returns>
        [Route("{instanceId:int}/Instance")]
        [HttpGet]
        [ProducesResponseType(typeof(GetLandInstanceCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLandInstance([FromRoute] int instanceId)
        {
            GetLandInstanceCommandResponse response = await mediator.Send(new GetLandInstanceCommandRequest(instanceId));

            return Ok(response);
        }

        /// <summary>
        /// Gets the user lands.
        /// </summary>
        /// <returns></returns>
        [Route("User")]
        [HttpGet]
        [ProducesResponseType(typeof(GetUserLandsCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserLands()
        {
            GetUserLandsCommandResponse response = await mediator.Send(new GetUserLandsCommandRequest(UserId));

            return Ok(response);
        }

        /// <summary>
        /// Discover the land.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Discover")]
        [HttpPut]
        [ProducesResponseType(typeof(DiscoverLandCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DiscoverLand([FromBody] DiscoverLandCommandRequest request)
        {
            request.UserId = UserId;
            DiscoverLandCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Explore the land.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Explore")]
        [HttpPut]
        [ProducesResponseType(typeof(ExploreLandCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ExploreLand([FromBody] ExploreLandCommandRequest request)
        {
            request.UserId = UserId;
            ExploreLandCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Explore the land.
        /// </summary>
        /// <param name="landId">The request.</param>
        /// <returns></returns>
        [Route("Purchase/Check/{landId:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(CheckPurchaseLandCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckPurchaseLand([FromRoute] long landId)
        {
            CheckPurchaseLandCommandResponse response = await mediator.Send(new CheckPurchaseLandCommandRequest(landId, UserId));

            return Accepted(response);
        }
    }
}
