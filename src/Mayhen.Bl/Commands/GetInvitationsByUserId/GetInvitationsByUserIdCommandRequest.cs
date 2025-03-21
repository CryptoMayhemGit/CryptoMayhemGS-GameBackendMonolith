using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetInvitationsByUserId
{
    public class GetInvitationsByUserIdCommandRequest : IRequest<GetInvitationsByUserIdCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public GetInvitationsByUserIdCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
