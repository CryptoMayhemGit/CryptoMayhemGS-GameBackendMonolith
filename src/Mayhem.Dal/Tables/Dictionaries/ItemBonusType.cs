using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class ItemBonusType : DictionaryTableBase<ItemBonusesType>
    {
        public ICollection<ItemBonus> ItemBonuses { get; set; }
    }
}
