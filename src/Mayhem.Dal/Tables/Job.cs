using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;
using System;

namespace Mayhem.Dal.Tables
{
    public class Job : TableBase
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public long NpcId { get; set; }
        public DateTime? StartDate { get; set; }

        public Land Land { get; set; }
        public Npc Npc { get; set; }
    }
}
