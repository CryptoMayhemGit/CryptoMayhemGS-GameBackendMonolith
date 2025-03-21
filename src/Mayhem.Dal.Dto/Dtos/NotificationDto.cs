using Mayhem.Dal.Dto.Dtos.Base;

namespace Mayhem.Dal.Dto.Dtos
{
    public class NotificationDto : TableBaseDto
    {
        public int Id { get; set; }
        public string WalletAddress { get; set; }
        public string Email { get; set; }
        public int Attempts { get; set; }
        public bool WasSent { get; set; }
    }
}
