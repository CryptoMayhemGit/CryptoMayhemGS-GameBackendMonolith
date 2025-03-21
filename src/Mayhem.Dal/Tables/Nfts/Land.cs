using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Missions;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Nfts
{
    public class Land : TableBase
    {
        public long Id { get; set; }
        public LandsType LandTypeId { get; set; }
        public int LandInstanceId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsMinted { get; set; }

        public LandInstance LandInstance { get; set; }
        public LandType LandType { get; set; }
        public ICollection<UserLand> UserLands { get; set; }
        public ICollection<Improvement> Improvements { get; set; }
        public ICollection<Npc> Npcs { get; set; }
        public ICollection<Travel> TravelsFrom { get; set; }
        public ICollection<Travel> TravelsTo { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Building> Buildings { get; set; }
        public ICollection<ExploreMission> ExploreMissions { get; set; }
        public ICollection<DiscoveryMission> DiscoveryMissions { get; set; }
    }
}
