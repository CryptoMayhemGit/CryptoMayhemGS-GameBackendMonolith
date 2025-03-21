using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.LeaveGuild
{
    public class LeaveGuildCommandRequest : IRequest<LeaveGuildCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public LeaveGuildCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
