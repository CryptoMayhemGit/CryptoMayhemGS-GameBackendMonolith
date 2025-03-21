using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class UserResourceDto : TableBaseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ResourcesType ResourceTypeId { get; set; }
        public double Value { get; set; }
    }
}
