using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.ReleaseItemFromNpc
{
    public class ReleaseItemFromNpcCommandRequest : IRequest<ReleaseItemFromNpcCommandResponse>
    {
        public long ItemId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
