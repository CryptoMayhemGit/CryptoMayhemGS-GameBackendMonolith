using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.MoveNpc
{
    public class MoveNpcCommandRequest : IRequest<MoveNpcCommandResponse>
    {
        public long LandToId { get; set; }
        public long NpcId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
