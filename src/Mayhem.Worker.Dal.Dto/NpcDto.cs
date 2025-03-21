using Mayhem.Worker.Dal.Dto.Enums;

namespace Mayhem.Worker.Dal.Dto
{
    public class NpcDto
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public long? LandId { get; set; }
        public NpcsStatus NpcStatusId { get; set; }
    }
}
