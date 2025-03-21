using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;
using System;

namespace Mayhem.Dal.Tables
{
    public class Travel : TableBase
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandFromId { get; set; }
        public long LandToId { get; set; }
        public long LandTargetId { get; set; }
        public DateTime FinishDate { get; set; }

        public Land LandFrom { get; set; }
        public Land LandTo { get; set; }
        public Npc Npc { get; set; }
    }
}
