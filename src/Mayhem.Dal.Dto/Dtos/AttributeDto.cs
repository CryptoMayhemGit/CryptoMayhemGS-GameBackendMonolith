using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class AttributeDto : TableBaseDto
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public AttributesType AttributeTypeId { get; set; }
        public double BaseValue { get; set; }
        public double Value { get; set; }
    }
}
