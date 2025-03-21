using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Guilds;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class ResourceType : DictionaryTableBase<ResourcesType>
    {
        public ICollection<UserResource> UserResources { get; set; }
        public ICollection<GuildResource> GuildResources { get; set; }
    }
}
