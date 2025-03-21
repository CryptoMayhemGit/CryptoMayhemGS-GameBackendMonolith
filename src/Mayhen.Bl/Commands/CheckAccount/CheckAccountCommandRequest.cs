using MediatR;

namespace Mayhen.Bl.Commands.CheckAccount
{
    public class CheckAccountCommandRequest : IRequest
    {
        public string WalletAddress { get; set; }
    }
}
