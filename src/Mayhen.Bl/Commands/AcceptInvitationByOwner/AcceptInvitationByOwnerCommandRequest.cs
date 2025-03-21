using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AcceptInvitationByOwner
{
    public class AcceptInvitationByOwnerCommandRequest : IRequest<AcceptInvitationByOwnerCommandResponse>
    {
        public int InvitationId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
