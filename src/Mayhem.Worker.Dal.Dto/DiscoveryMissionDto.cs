using System;

namespace Mayhem.Worker.Dal.Dto
{
    public class DiscoveryMissionDto
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandId { get; set; }
        public int UserId { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
