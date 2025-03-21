using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetGuildBuildingList
{
    public class GetGuildBuildingListCommandResponse
    {
        public IEnumerable<GuildBuildingDto> GuildBuildings { get; set; }
    }
}
