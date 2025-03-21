using Mayhem.Dal.Dto.Dtos.Base;
using System;

namespace Mayhem.Dal.Dto.Dtos
{
    public class JobDto : TableBaseDto
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public long NpcId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
