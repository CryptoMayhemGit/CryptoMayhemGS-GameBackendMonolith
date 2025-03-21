using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables
{
    public class Block : TableBase
    {
        public int Id { get; set; }
        public long LastBlock { get; set; }
        public BlocksType BlockTypeId { get; set; }

        public BlockType BlockType { get; set; }
    }
}
