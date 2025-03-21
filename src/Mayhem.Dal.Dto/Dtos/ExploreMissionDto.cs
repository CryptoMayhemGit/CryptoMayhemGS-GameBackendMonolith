using Mayhem.Dal.Dto.Dtos.Base;
using System;

namespace Mayhem.Dal.Dto.Dtos
{
    public class ExploreMissionDto : TableBaseDto
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public long LandId { get; set; }
        public int UserId { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
