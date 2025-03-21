using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.AddBuildingToLand;
using Mayhen.Bl.Commands.AddGuildBuilding;
using Mayhen.Bl.Commands.GetBuildingList;
using Mayhen.Bl.Commands.GetGuildBuildingList;
using Mayhen.Bl.Commands.UpgradeBuilding;
using Mayhen.Bl.Commands.UpgradeGuildBuilding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Building)]
    public class BuildingController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public BuildingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Add building to land.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddBuildingToLandCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddBuildingToLand([FromBody] AddBuildingToLandCommandRequest request)
        {
            request.UserId = UserId;
            AddBuildingToLandCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(AddBuildingToLand), response);
        }

        /// <summary>
        /// Add building to guild.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Guild/Add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddGuildBuildingCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddGuildBuilding([FromBody] AddGuildBuildingCommandRequest request)
        {
            request.UserId = UserId;
            AddGuildBuildingCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(AddGuildBuilding), response);
        }


        /// <summary>
        /// Get building list of a specific land type.
        /// </summary>
        /// <param name="landType">Type of the land.</param>
        /// <returns></returns>
        [Route("{landType}/List")]
        [HttpGet]
        [ProducesResponseType(typeof(GetBuildingListCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBuildingList([FromRoute] LandsType landType)
        {
            GetBuildingListCommandResponse response = await mediator.Send(new GetBuildingListCommandRequest(landType));

            return Ok(response);
        }

        /// <summary>
        /// Get guild buildings.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        [Route("Guild/{guildId:int}/List")]
        [HttpGet]
        [ProducesResponseType(typeof(GetGuildBuildingListCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGuildBuildingList([FromRoute] int guildId)
        {
            GetGuildBuildingListCommandResponse response = await mediator.Send(new GetGuildBuildingListCommandRequest(guildId));

            return Ok(response);
        }

        /// <summary>
        /// Upgrade building.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Upgrade")]
        [HttpPut]
        [ProducesResponseType(typeof(UpgradeBuildingCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpgradeBuilding([FromBody] UpgradeBuildingCommandRequest request)
        {
            request.UserId = UserId;
            UpgradeBuildingCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Upgrade guild building.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Guild/Upgrade")]
        [HttpPut]
        [ProducesResponseType(typeof(UpgradeGuildBuildingCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpgradeGuildBuilding([FromBody] UpgradeGuildBuildingCommandRequest request)
        {
            request.UserId = UserId;
            UpgradeGuildBuildingCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }
    }
}