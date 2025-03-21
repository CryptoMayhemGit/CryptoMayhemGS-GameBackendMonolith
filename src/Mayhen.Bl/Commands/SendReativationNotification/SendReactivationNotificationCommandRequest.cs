using MediatR;

namespace Mayhen.Bl.Commands.SendReativationNotification
{
    public class SendReactivationNotificationCommandRequest : IRequest<SendReactivationNotificationCommandResponse>
    {
        public string Email { get; set; }
    }
}
