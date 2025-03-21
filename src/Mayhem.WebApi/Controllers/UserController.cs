using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.User)]
    public class UserController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get user with specific information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(GetUserCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUser([FromQuery] GetUserCommandRequest request)
        {
            request.UserId = UserId;
            GetUserCommandResponse response = await mediator.Send(request);
            return Ok(response);
        }
    }
}
