using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.CheckAccount;
using Mayhen.Bl.Commands.CheckEmail;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Commands.SendReativationNotification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [ApiController]
    [Route("api/" + ControllerNames.Account)]
    public class AccountController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Login via metamask signature and get token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
        {
            LoginCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(Login), response);
        }

        /// <summary>
        /// Create account by wallet address.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Register([FromBody] RegisterCommandRequest request)
        {
            RegisterCommandResponse response = await mediator.Send(request);

            return !response.Success ? NotFound() : CreatedAtAction(nameof(Register), response);
        }

        /// <summary>
        /// Send activation notification to new user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Activation")]
        [HttpPost]
        [ProducesResponseType(typeof(SendActivationNotificationCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SendActivationNotification([FromBody] SendActivationNotificationCommandRequest request)
        {
            SendActivationNotificationCommandResponse response = await mediator.Send(request);

            return !response.Success ? NotFound() : CreatedAtAction(nameof(SendActivationNotification), response);
        }

        /// <summary>
        /// Resends the activation link if the activation link has already been sent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Reactivation")]
        [HttpPost]
        [ProducesResponseType(typeof(SendReactivationNotificationCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ResendActivationNotification([FromBody] SendReactivationNotificationCommandRequest request)
        {
            SendReactivationNotificationCommandResponse response = await mediator.Send(request);

            return !response.Success ? NotFound() : CreatedAtAction(nameof(ResendActivationNotification), response);
        }

        /// <summary>
        /// Verify the email.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Verify/Email")]
        [HttpPost]
        [ProducesResponseType(typeof(CheckEmailCommandResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailCommandRequest request)
        {
            CheckEmailCommandResponse response = await mediator.Send(request);

            return Ok(response);
        }


        /// <summary>
        /// Check the account. Checks if an account has not yet been created for the specified wallet.
        /// The check is carried out on the basis of active users and sent notifications. 
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Check")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Check([FromBody] CheckAccountCommandRequest request)
        {
            await mediator.Send(request);

            return Ok();
        }
    }
}
