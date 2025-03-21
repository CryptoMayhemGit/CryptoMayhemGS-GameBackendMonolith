using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class ItemDto : TableBaseDto
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public ItemsType ItemTypeId { get; set; }
        public bool IsUsed { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsMinted { get; set; }

        public ICollection<ItemBonusDto> ItemBonuses { get; set; }

    }
}
