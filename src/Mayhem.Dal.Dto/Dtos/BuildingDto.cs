using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class BuildingDto : TableBaseDto
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public int Level { get; set; }
        public BuildingsType BuildingTypeId { get; set; }

        public ICollection<BuildingBonusDto> BuildingBonuses { get; set; }
    }
}
