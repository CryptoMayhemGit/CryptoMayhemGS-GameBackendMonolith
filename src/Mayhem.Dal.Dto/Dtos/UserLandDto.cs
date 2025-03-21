using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class UserLandDto : TableBaseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long LandId { get; set; }
        public bool Owned { get; set; }
        public bool HasFog { get; set; }
        public LandsStatus Status { get; set; }
        public bool OnSale { get; set; }

        public LandDto Land { get; set; }
    }
}
