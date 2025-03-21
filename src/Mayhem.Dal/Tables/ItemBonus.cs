using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Nfts;

namespace Mayhem.Dal.Tables
{
    public class ItemBonus : TableBase
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public ItemBonusesType ItemBonusTypeId { get; set; }
        public double Bonus { get; set; }

        public Item Item { get; set; }
        public ItemBonusType ItemBonusType { get; set; }
    }
}
