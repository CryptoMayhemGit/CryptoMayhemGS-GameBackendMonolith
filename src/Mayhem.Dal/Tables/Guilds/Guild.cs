using Mayhem.Dal.Tables.Base;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Guilds
{
    public class Guild : TableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }

        public GameUser Owner { get; set; }
        public ICollection<GuildResource> GuildResources { get; set; }
        public ICollection<GameUser> Users { get; set; }
        public ICollection<GuildInvitation> GuildInvitations { get; set; }
        public ICollection<GuildBuilding> GuildBuildings { get; set; }
        public ICollection<GuildImprovement> GuildImprovements { get; set; }
    }
}
