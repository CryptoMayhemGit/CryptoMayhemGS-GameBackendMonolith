using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;

namespace Mayhem.Dal.Tables
{
    public class UserLand : TableBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long LandId { get; set; }
        public bool Owned { get; set; }
        public bool HasFog { get; set; }
        public LandsStatus Status { get; set; }
        public bool OnSale { get; set; }

        public GameUser User { get; set; }
        public Land Land { get; set; }
    }
}
