using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.DiscoverLand
{
    public class DiscoverLandCommandRequest : IRequest<DiscoverLandCommandResponse>
    {
        public long NpcId { get; set; }
        public long LandId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
