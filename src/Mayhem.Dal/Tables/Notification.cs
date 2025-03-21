using Mayhem.Dal.Tables.Base;

namespace Mayhem.Dal.Tables
{
    public class Notification : TableBase
    {
        public int Id { get; set; }
        public string WalletAddress { get; set; }
        public string Email { get; set; }
        public int Attempts { get; set; }
        public bool WasSent { get; set; }
    }
}
