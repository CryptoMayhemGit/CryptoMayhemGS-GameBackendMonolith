using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Nfts
{
    public class Item : TableBase
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public ItemsType ItemTypeId { get; set; }
        public bool IsUsed { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsMinted { get; set; }

        public GameUser GameUser { get; set; }
        public Npc Npc { get; set; }
        public ItemType ItemType { get; set; }
        public ICollection<ItemBonus> ItemBonuses { get; set; }
    }
}
