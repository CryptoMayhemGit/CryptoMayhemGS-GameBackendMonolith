using Mayhem.Worker.Dal.Dto.Enums;

namespace Mayhem.Worker.Dal.Dto
{
    public class UserLandDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long LandId { get; set; }
        public bool Owned { get; set; }
        public bool HasFog { get; set; }
        public LandsStatus Status { get; set; }
        public bool OnSale { get; set; }
    }
}
