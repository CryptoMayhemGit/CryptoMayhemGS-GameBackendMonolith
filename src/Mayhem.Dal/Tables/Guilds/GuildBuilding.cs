using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Guilds
{
    public class GuildBuilding : TableBase
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int GuildId { get; set; }
        public GuildBuildingsType GuildBuildingTypeId { get; set; }

        public Guild Guild { get; set; }
        public GuildBuildingType GuildBuildingType { get; set; }
        public ICollection<GuildBuildingBonus> GuildBuildingBonuses { get; set; }
    }
}
