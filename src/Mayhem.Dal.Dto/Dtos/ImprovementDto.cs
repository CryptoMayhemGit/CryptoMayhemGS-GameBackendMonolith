using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class ImprovementDto : TableBaseDto
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public ImprovementsType ImprovementTypeId { get; set; }
    }
}
