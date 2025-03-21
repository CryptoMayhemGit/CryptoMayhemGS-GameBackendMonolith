using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.ChangeGuildOwner
{
    public class ChangeGuildOwnerCommandRequest : IRequest<ChangeGuildOwnerCommandResponse>
    {
        public int NewOwnerId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
