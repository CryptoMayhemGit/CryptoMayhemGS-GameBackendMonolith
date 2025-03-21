using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class AttributeType : DictionaryTableBase<AttributesType>
    {
        public ICollection<Attribute> Attributes { get; set; }
    }
}
