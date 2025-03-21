using Mayhem.Util.Classes;
using MediatR;

namespace Mayhen.Bl.Commands.Base
{
    public class WalletCommandRequest<T> : IRequest<T>
    {
        public string Wallet { get; set; }
        public string SignedMessage { get; set; }
        public MessageToSign MessageToSign { get; set; }
    }
}
