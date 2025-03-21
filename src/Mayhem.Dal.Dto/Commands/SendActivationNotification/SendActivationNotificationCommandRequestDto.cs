namespace Mayhem.Dal.Dto.Commands.SendActivationNotification
{
    public class SendActivationNotificationCommandRequestDto
    {
        public string Wallet { get; set; }
        public string Email { get; set; }
    }
}
