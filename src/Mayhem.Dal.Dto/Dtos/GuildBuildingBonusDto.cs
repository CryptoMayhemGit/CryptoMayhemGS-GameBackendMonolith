using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GuildBuildingBonusDto : TableBaseDto
    {
        public int Id { get; set; }
        public int GuildBuildingId { get; set; }
        public GuildBuildingBonusesType GuildBuildingBonusTypeId { get; set; }
        public double Bonus { get; set; }
    }
}
