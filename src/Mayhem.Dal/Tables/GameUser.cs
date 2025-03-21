using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;
using System;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables
{
    public class GameUser : TableBase
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string WalletAddress { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? GuildId { get; set; }

        public Guild GuildMember { get; set; }
        public Guild GuildOwner { get; set; }
        public ICollection<UserLand> UserLands { get; set; }
        public ICollection<UserResource> UserResources { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Npc> Npcs { get; set; }
        public ICollection<GuildInvitation> GuildInvitations { get; set; }
        public ICollection<DiscoveryMission> DiscoveryMissions { get; set; }
        public ICollection<ExploreMission> ExploreMissions { get; set; }
    }
}
