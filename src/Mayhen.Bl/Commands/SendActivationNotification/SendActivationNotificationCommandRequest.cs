using Mayhen.Bl.Commands.Base;

namespace Mayhen.Bl.Commands.SendActivationNotification
{
    public class SendActivationNotificationCommandRequest : WalletCommandRequest<SendActivationNotificationCommandResponse>
    {
        public string Email { get; set; }
    }
}
