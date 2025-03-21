using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.CheckPath
{
    public class CheckPathCommandRequest : IRequest<CheckPathCommandResponse>
    {
        public long LandFromId { get; set; }
        public long LandToId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
