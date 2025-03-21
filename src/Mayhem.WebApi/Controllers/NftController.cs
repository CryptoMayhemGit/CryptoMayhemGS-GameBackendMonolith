using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.GetNftItem;
using Mayhen.Bl.Commands.GetNftLand;
using Mayhen.Bl.Commands.GetNftNpc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [ApiController]
    [Route("api/" + ControllerNames.Nft)]
    public class NftController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public NftController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get item nft data.
        /// </summary>
        /// <param name="itemNftId">The item nft identifier.</param>
        /// <returns></returns>
        [Route("Item/{itemNftId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetNftItemCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetNftItem([FromRoute] int itemNftId)
        {
            GetNftItemCommandResponse response = await mediator.Send(new GetNftItemCommandRequest(itemNftId));

            return response == null ? NotFound() : Ok(response.Model);
        }

        /// <summary>
        /// Get land nft data.
        /// </summary>
        /// <param name="landNftId">The land nft identifier.</param>
        /// <returns></returns>
        [Route("Land/{landNftId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetNftLandCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetNftLand([FromRoute] int landNftId)
        {
            GetNftLandCommandResponse response = await mediator.Send(new GetNftLandCommandRequest(landNftId));

            return response == null ? NotFound() : Ok(response.Model);
        }

        /// <summary>
        /// Get hero nft data.
        /// </summary>
        /// <param name="heroNftId">The Hero nft identifier.</param>
        /// <returns></returns>
        [Route("Hero/{heroNftId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetNftNpcCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetNftNpc([FromRoute] int heroNftId)
        {
            GetNftNpcCommandResponse response = await mediator.Send(new GetNftNpcCommandRequest(heroNftId));

            return response == null ? NotFound() : Ok(response.Model);
        }
    }
}
