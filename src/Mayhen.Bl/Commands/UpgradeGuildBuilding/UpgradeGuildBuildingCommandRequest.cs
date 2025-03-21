using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.UpgradeGuildBuilding
{
    public class UpgradeGuildBuildingCommandRequest : IRequest<UpgradeGuildBuildingCommandResponse>
    {
        public int GuildBuildingId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
