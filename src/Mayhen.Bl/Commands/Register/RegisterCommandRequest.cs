using MediatR;

namespace Mayhen.Bl.Commands.Register
{
    public class RegisterCommandRequest : IRequest<RegisterCommandResponse>
    {
        public string ActivationNotificationToken { get; set; }
    }
}
