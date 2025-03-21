using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AcceptInvitationByUser
{
    public class AcceptInvitationByUserCommandRequest : IRequest<AcceptInvitationByUserCommandResponse>
    {
        public int InvitationId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
