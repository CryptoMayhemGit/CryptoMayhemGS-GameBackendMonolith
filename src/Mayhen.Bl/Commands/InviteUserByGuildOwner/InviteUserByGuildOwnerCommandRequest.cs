using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.InviteUserByGuildOwner
{
    public class InviteUserByGuildOwnerCommandRequest : IRequest<InviteUserByGuildOwnerCommandResponse>
    {
        public int InvitedUserId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
