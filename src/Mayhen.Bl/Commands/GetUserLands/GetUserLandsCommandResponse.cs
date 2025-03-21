using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetUserLands
{
    public class GetUserLandsCommandResponse
    {
        public IEnumerable<UserLandDto> UserLands { get; set; }
        public IEnumerable<UserLandDto> OwnedLands { get; set; }
        public IEnumerable<BuildingDto> UserBuildings { get; set; }
        public IEnumerable<BuildingDto> EnemyBuildings { get; set; }
        public IEnumerable<UserLandDto> EnemyLands { get; set; }
        public IEnumerable<NpcDto> UserNpcs { get; set; }
        public IEnumerable<NpcDto> EnemyNpcs { get; set; }
    }
}
