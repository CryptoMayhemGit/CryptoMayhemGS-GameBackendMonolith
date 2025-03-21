using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Missions;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Nfts
{
    public class Npc : TableBase
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long? BuildingId { get; set; }
        public NpcsType NpcTypeId { get; set; }
        public NpcHealthsState NpcHealthStateId { get; set; }
        public bool IsAvatar { get; set; }
        public long? ItemId { get; set; }
        public bool IsMinted { get; set; }
        public long? LandId { get; set; }
        public NpcsStatus NpcStatusId { get; set; }

        public Land Land { get; set; }
        public Building Building { get; set; }
        public GameUser GameUser { get; set; }
        public Item Item { get; set; }
        public Job Job { get; set; }
        public NpcType NpcType { get; set; }
        public NpcStatus NpcStatus { get; set; }
        public ExploreMission ExploreMission { get; set; }
        public DiscoveryMission DiscoveryMission { get; set; }
        public ICollection<Travel> Travels { get; set; }
        public ICollection<Attribute> Attributes { get; set; }
    }
}
