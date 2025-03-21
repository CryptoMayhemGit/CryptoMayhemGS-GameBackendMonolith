using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables.Buildings
{
    public class BuildingBonus : TableBase
    {
        public long Id { get; set; }
        public long BuildingId { get; set; }
        public BuildingBonusesType BuildingBonusTypeId { get; set; }
        public double Bonus { get; set; }

        public Building Building { get; set; }
        public BuildingBonusType BuildingBonusType { get; set; }
    }
}
