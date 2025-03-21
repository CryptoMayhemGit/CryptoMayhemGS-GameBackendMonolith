using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Buildings;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class BuildingBonusType : DictionaryTableBase<BuildingBonusesType>
    {
        public BuildingBonus BuildingBonus { get; set; }
    }
}
