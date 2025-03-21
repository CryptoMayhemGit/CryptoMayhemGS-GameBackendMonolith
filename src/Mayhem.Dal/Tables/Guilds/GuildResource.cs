using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables.Guilds
{
    public class GuildResource : TableBase
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public ResourcesType ResourceTypeId { get; set; }
        public double Value { get; set; }

        public ResourceType ResourceType { get; set; }
        public Guild Guild { get; set; }
    }
}
