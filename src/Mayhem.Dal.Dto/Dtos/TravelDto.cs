using Mayhem.Dal.Dto.Dtos.Base;
using System;

namespace Mayhem.Dal.Dto.Dtos
{
    public class TravelDto : TableBaseDto
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandFromId { get; set; }
        public long LandToId { get; set; }
        public long LandTargetId { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
