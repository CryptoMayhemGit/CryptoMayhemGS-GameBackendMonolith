using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class ItemBonusDto : TableBaseDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public ItemBonusesType ItemBonusTypeId { get; set; }
        public double Bonus { get; set; }
    }
}
