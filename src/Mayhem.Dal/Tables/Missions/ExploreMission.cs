using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;
using System;

namespace Mayhem.Dal.Tables.Missions
{
    public class ExploreMission : TableBase
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandId { get; set; }
        public int UserId { get; set; }
        public DateTime FinishDate { get; set; }

        public Land Land { get; set; }
        public Npc Npc { get; set; }
        public GameUser User { get; set; }
    }
}
