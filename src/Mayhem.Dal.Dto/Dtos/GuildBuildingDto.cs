using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GuildBuildingDto : TableBaseDto
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int GuildId { get; set; }
        public GuildBuildingsType GuildBuildingTypeId { get; set; }

        public ICollection<GuildBuildingBonusDto> GuildBuildingBonuses { get; set; }
    }
}
