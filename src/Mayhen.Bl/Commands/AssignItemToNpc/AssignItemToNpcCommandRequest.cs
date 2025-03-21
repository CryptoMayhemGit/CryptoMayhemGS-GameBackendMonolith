using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AssignItemToNpc
{
    public class AssignItemToNpcCommandRequest : IRequest<AssignItemToNpcCommandResponse>
    {
        public long ItemId { get; set; }
        public long NpcId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
