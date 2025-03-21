using System;

namespace Mayhem.Worker.Dal.Dto
{
    public class TravelDto
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandFromId { get; set; }
        public long LandToId { get; set; }
        public long LandTargetId { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
