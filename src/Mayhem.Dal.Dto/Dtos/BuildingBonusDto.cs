using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class BuildingBonusDto : TableBaseDto
    {
        public long Id { get; set; }
        public long BuildingId { get; set; }
        public BuildingBonusesType BuildingBonusTypeId { get; set; }
        public double Bonus { get; set; }
    }
}
