using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Nfts;

namespace Mayhem.Dal.Tables
{
    public class Attribute : TableBase
    {
        public long Id { get; set; }
        public long NpcId { get; set; }
        public AttributesType AttributeTypeId { get; set; }
        public double BaseValue { get; set; }
        public double Value { get; set; }

        public Npc Npc { get; set; }
        public AttributeType AttributeType { get; set; }
    }
}
