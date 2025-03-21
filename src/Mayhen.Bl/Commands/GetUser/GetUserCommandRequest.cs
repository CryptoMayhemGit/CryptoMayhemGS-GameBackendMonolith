using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetUser
{
    public class GetUserCommandRequest : IRequest<GetUserCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public bool WithResources { get; set; }
        public bool WithItems { get; set; }
        public bool WithNpcs { get; set; }
        public bool WithLands { get; set; }
    }
}
