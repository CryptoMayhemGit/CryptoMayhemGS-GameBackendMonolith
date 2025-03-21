using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.UpgradeBuilding
{
    public class UpgradeBuildingCommandRequest : IRequest<UpgradeBuildingCommandResponse>
    {
        public long BuildingId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
