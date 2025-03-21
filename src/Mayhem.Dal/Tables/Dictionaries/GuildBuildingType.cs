using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Guilds;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class GuildBuildingType : DictionaryTableBase<GuildBuildingsType>
    {
        public ICollection<GuildBuilding> GuildBuildings { get; set; }
    }
}
