using Mayhem.Blockchain.Enums;

namespace Mayhem.Worker.Dal.Dto
{
    public class BlockDto
    {
        public int Id { get; set; }
        public long LastBlock { get; set; }
        public BlocksType BlockTypeId { get; set; }
    }
}
