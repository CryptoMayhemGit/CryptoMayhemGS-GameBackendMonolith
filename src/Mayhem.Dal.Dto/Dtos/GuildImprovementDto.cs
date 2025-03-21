using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GuildImprovementDto : TableBaseDto
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public GuildImprovementsType GuildImprovementTypeId { get; set; }
    }
}
