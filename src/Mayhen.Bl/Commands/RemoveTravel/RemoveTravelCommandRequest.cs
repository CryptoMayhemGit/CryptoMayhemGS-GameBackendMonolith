using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.RemoveTravel
{
    public class RemoveTravelCommandRequest : IRequest<RemoveTravelCommandResponse>
    {
        public long NpcId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
