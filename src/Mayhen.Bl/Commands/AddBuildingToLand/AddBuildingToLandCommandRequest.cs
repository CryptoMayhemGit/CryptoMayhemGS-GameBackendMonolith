using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AddBuildingToLand
{
    public class AddBuildingToLandCommandRequest : IRequest<AddBuildingToLandCommandResponse>
    {
        public BuildingsType BuildingTypeId { get; set; }
        public long LandId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
