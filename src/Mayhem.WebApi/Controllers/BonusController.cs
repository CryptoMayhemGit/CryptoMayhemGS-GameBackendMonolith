using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.GetMechBonus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Bonus)]
    public class BonusController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public BonusController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetMechBonusCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMechBonus()
        {
            GetMechBonusCommandResponse response = await mediator.Send(new GetMechBonusCommandRequest(UserId));
            return Ok(response);
        }
    }
}
