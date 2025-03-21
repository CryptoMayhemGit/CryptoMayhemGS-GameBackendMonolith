using Mayhem.Dal.Dto.Dtos.Base;

namespace Mayhem.Dal.Dto.Dtos
{
    public class AuditLogDto : TableBaseDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Wallet { get; set; }
        public string SignedMessage { get; set; }
        public string Message { get; set; }
        public long Nonce { get; set; }
    }
}
