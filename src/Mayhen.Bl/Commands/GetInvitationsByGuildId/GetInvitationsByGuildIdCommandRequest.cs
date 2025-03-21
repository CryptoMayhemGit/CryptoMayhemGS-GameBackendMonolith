using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetInvitationsByGuildId
{
    public class GetInvitationsByGuildIdCommandRequest : IRequest<GetInvitationsByGuildIdCommandResponse>
    {
        public int GuildId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public GetInvitationsByGuildIdCommandRequest(int guildId, int userId)
        {
            GuildId = guildId;
            UserId = userId;
        }
    }
}
