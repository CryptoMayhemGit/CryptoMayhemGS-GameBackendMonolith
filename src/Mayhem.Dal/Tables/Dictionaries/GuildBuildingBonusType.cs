using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Guilds;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class GuildBuildingBonusType : DictionaryTableBase<GuildBuildingBonusesType>
    {
        public GuildBuildingBonus GuildBuildingBonus { get; set; }
    }
}
