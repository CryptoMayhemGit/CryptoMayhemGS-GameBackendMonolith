using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.ExploreLand
{
    public class ExploreLandCommandRequest : IRequest<ExploreLandCommandResponse>
    {
        public long NpcId { get; set; }
        public long LandId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
