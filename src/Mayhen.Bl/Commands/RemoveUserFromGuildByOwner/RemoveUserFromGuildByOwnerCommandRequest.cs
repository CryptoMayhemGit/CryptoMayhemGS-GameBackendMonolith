using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.RemoveUserFromGuildByOwner
{
    public class RemoveUserFromGuildByOwnerCommandRequest : IRequest<RemoveUserFromGuildByOwnerCommandResponse>
    {
        public int RemovedUserId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
