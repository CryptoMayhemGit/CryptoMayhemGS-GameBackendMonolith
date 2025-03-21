using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Buildings;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class BuildingType : DictionaryTableBase<BuildingsType>
    {
        public ICollection<Building> Buildings { get; set; }
    }
}
