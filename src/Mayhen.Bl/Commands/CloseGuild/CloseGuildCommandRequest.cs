using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.CloseGuild
{
    public class CloseGuildCommandRequest : IRequest<CloseGuildCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public CloseGuildCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
