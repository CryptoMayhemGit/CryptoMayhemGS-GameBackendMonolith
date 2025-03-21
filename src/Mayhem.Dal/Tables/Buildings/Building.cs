using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Nfts;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Buildings
{
    public class Building : TableBase
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public int Level { get; set; }
        public BuildingsType BuildingTypeId { get; set; }

        public Land Land { get; set; }
        public BuildingType BuildingType { get; set; }
        public ICollection<Npc> Npcs { get; set; }
        public ICollection<BuildingBonus> BuildingBonuses { get; set; }
    }
}
