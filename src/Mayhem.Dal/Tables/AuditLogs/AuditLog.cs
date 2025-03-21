using Mayhem.Dal.Tables.Base;

namespace Mayhem.Dal.Tables.AuditLogs
{
    public class AuditLog : TableBase
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Wallet { get; set; }
        public string SignedMessage { get; set; }
        public string Message { get; set; }
        public long Nonce { get; set; }
    }
}
