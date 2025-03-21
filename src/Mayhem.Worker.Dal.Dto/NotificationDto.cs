namespace Mayhem.Worker.Dal.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int Attempts { get; set; }
        public string Email { get; set; }
        public string WalletAddress { get; set; }
    }
}
