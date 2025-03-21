using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class BlockType : DictionaryTableBase<BlocksType>
    {
        public ICollection<Block> Blocks { get; set; }
    }
}
