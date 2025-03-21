using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AddGuildBuilding
{
    public class AddGuildBuildingCommandRequest : IRequest<AddGuildBuildingCommandResponse>
    {
        public GuildBuildingsType GuildBuildingTypeId { get; set; }
        public int GuildId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
