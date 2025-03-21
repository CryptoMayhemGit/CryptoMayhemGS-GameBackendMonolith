using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.DeclineInvitationByUser
{
    public class DeclineInvitationByUserCommandRequest : IRequest<DeclineInvitationByUserCommandResponse>
    {
        public int InvitationId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
