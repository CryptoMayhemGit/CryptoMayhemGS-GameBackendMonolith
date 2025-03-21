using MediatR;

namespace Mayhen.Bl.Commands.CheckEmail
{
    public class CheckEmailCommandRequest : IRequest<CheckEmailCommandResponse>
    {
        public string Email { get; set; }
    }
}
