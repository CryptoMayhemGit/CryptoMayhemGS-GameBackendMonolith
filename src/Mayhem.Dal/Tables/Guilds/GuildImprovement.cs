using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables.Guilds
{
    public class GuildImprovement : TableBase
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public GuildImprovementsType GuildImprovementTypeId { get; set; }

        public Guild Guild { get; set; }
        public GuildImprovementType GuildImprovementType { get; set; }
    }
}
