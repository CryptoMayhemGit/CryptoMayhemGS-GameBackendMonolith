using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.DeclineInvitationByOwner
{
    public class DeclineInvitationByOwnerCommandRequest : IRequest<DeclineInvitationByOwnerCommandResponse>
    {
        public int InvitationId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
