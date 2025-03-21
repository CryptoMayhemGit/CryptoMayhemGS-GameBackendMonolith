using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AsksToJoinGuildByUser
{
    public class AskToJoinGuildByUserCommandRequest : IRequest<AskToJoinGuildByUserCommandResponse>
    {
        public int GuildId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
