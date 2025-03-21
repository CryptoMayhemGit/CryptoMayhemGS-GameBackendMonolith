using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class NpcDto : TableBaseDto
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long? BuildingId { get; set; }
        public NpcsType NpcTypeId { get; set; }
        public NpcHealthsState NpcHealthStateId { get; set; }
        public bool IsAvatar { get; set; }
        public long? ItemId { get; set; }
        public bool IsMinted { get; set; }
        public long? LandId { get; set; }
        public NpcsStatus NpcStatusId { get; set; }
        public ICollection<AttributeDto> Attributes { get; set; }
    }
}
