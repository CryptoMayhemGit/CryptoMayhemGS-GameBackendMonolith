using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables.Guilds
{
    public class GuildBuildingBonus : TableBase
    {
        public int Id { get; set; }
        public int GuildBuildingId { get; set; }
        public GuildBuildingBonusesType GuildBuildingBonusTypeId { get; set; }
        public double Bonus { get; set; }

        public GuildBuilding GuildBuilding { get; set; }
        public GuildBuildingBonusType GuildBuildingBonusType { get; set; }
    }
}
