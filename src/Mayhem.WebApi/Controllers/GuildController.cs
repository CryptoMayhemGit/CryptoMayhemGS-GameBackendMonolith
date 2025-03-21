using Mayhem.WebApi.ActionNames;
using Mayhem.WebApi.Base;
using Mayhen.Bl.Commands.AcceptInvitationByOwner;
using Mayhen.Bl.Commands.AcceptInvitationByUser;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Commands.ChangeGuildOwner;
using Mayhen.Bl.Commands.CloseGuild;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Commands.DeclineInvitationByOwner;
using Mayhen.Bl.Commands.DeclineInvitationByUser;
using Mayhen.Bl.Commands.GetGuildById;
using Mayhen.Bl.Commands.GetGuilds;
using Mayhen.Bl.Commands.GetInvitationsByGuildId;
using Mayhen.Bl.Commands.GetInvitationsByUserId;
using Mayhen.Bl.Commands.InviteUserByGuildOwner;
using Mayhen.Bl.Commands.LeaveGuild;
using Mayhen.Bl.Commands.RemoveUserFromGuildByOwner;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + ControllerNames.Guild)]
    public class GuildController : OwnControllerBase
    {
        private readonly IMediator mediator;

        public GuildController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the guilds.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetGuildsCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetGuilds([FromQuery] GetGuildsCommandRequest request)
        {
            GetGuildsCommandResponse response = await mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Get the guild by identifier.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        [Route("{guildId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetGuildByIdCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetGuildById([FromRoute] int guildId)
        {
            GetGuildByIdCommandResponse response = await mediator.Send(new GetGuildByIdCommandRequest(guildId));

            return Ok(response);
        }

        /// <summary>
        /// Get the invitations by guild identifier.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        [Route("{guildId:int}/Invitation")]
        [HttpGet]
        [ProducesResponseType(typeof(GetInvitationsByGuildIdCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetInvitationsByGuildId([FromRoute] int guildId)
        {
            GetInvitationsByGuildIdCommandResponse response = await mediator.Send(new GetInvitationsByGuildIdCommandRequest(guildId, UserId));

            return Ok(response);
        }

        /// <summary>
        /// Get the invitations by user identifier.
        /// </summary>
        /// <returns></returns>
        [Route("Invitation/User")]
        [HttpGet]
        [ProducesResponseType(typeof(GetInvitationsByUserIdCommandResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetInvitationsByUserId()
        {
            GetInvitationsByUserIdCommandResponse response = await mediator.Send(new GetInvitationsByUserIdCommandRequest(UserId));

            return Ok(response);
        }

        /// <summary>
        /// Accept the invitation by owner.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/Owner/Accept")]
        [HttpPut]
        [ProducesResponseType(typeof(AcceptInvitationByOwnerCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AcceptInvitationByOwner([FromBody] AcceptInvitationByOwnerCommandRequest request)
        {
            request.UserId = UserId;
            AcceptInvitationByOwnerCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Accept the invitation by user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/User/Accept")]
        [HttpPut]
        [ProducesResponseType(typeof(AcceptInvitationByUserCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AcceptInvitationByUser([FromBody] AcceptInvitationByUserCommandRequest request)
        {
            request.UserId = UserId;
            AcceptInvitationByUserCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Ask to join guild by user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/User/Send")]
        [HttpPost]
        [ProducesResponseType(typeof(AskToJoinGuildByUserCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AskToJoinGuildByUser([FromBody] AskToJoinGuildByUserCommandRequest request)
        {
            request.UserId = UserId;
            AskToJoinGuildByUserCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(AskToJoinGuildByUser), response);
        }

        /// <summary>
        /// Invite the user by guild owner.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/Owner/Send")]
        [HttpPost]
        [ProducesResponseType(typeof(InviteUserByGuildOwnerCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> InviteUserByGuildOwner([FromBody] InviteUserByGuildOwnerCommandRequest request)
        {
            request.UserId = UserId;
            InviteUserByGuildOwnerCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(InviteUserByGuildOwner), response);
        }

        /// <summary>
        /// Change the guild owner.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Owner/Change")]
        [HttpPut]
        [ProducesResponseType(typeof(ChangeGuildOwnerCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeGuildOwner([FromBody] ChangeGuildOwnerCommandRequest request)
        {
            request.UserId = UserId;
            ChangeGuildOwnerCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Close the guild.
        /// </summary>
        /// <returns></returns>
        [Route("Close")]
        [HttpDelete]
        [ProducesResponseType(typeof(CloseGuildCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CloseGuild()
        {
            CloseGuildCommandResponse response = await mediator.Send(new CloseGuildCommandRequest(UserId));

            return Accepted(response);
        }

        /// <summary>
        /// Create the guild.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateGuildCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateGuild([FromBody] CreateGuildCommandRequest request)
        {
            request.UserId = UserId;
            CreateGuildCommandResponse response = await mediator.Send(request);

            return CreatedAtAction(nameof(CreateGuild), response);
        }

        /// <summary>
        /// Decline the invitation by owner.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/Owner/Decline")]
        [HttpDelete]
        [ProducesResponseType(typeof(DeclineInvitationByOwnerCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeclineInvitationByOwner([FromBody] DeclineInvitationByOwnerCommandRequest request)
        {
            request.UserId = UserId;
            DeclineInvitationByOwnerCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Decline the invitation by user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("Invitation/User/Decline")]
        [HttpDelete]
        [ProducesResponseType(typeof(DeclineInvitationByUserCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeclineInvitationByUser([FromBody] DeclineInvitationByUserCommandRequest request)
        {
            request.UserId = UserId;
            DeclineInvitationByUserCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }

        /// <summary>
        /// Leave the guild.
        /// </summary>
        /// <returns></returns>
        [Route("User/Leave")]
        [HttpDelete]
        [ProducesResponseType(typeof(InviteUserByGuildOwnerCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> LeaveGuild()
        {
            LeaveGuildCommandResponse response = await mediator.Send(new LeaveGuildCommandRequest(UserId));

            return Accepted(response);
        }

        /// <summary>
        /// Remove the user from guild by owner.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("User/Remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(InviteUserByGuildOwnerCommandResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveUserFromGuildByOwner([FromBody] RemoveUserFromGuildByOwnerCommandRequest request)
        {
            request.UserId = UserId;
            RemoveUserFromGuildByOwnerCommandResponse response = await mediator.Send(request);

            return Accepted(response);
        }
    }
}
