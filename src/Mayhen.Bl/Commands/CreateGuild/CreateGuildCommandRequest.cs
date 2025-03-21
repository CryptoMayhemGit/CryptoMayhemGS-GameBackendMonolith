using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.CreateGuild
{
    public class CreateGuildCommandRequest : IRequest<CreateGuildCommandResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
