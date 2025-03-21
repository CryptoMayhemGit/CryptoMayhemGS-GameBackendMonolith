using Mayhem.Util.Classes;
using Mayhem.WebApi.Dto.SwaggerExamples.Base;
using Mayhen.Bl.Commands.Login;

namespace Mayhem.WebApi.Dto.SwaggerExamples
{
    public class ExampleLoginCommandRequest : ExampleBase<LoginCommandRequest>
    {
        private const string PrivateKey = "0xa302447dc83d1418360c91080791169f37ddaf1b6ced07315e687a596b0fc1ac";
        private const string Wallet = "0xe62e0ccc74cb7b4c7af31b096d72360c3c20b696";

        public override LoginCommandRequest GetExamples()
        {
            (string signedMessage, string messageToSign, long nonce) = GetSignature(PrivateKey, "test message");

            return new LoginCommandRequest()
            {
                Wallet = Wallet,
                SignedMessage = signedMessage,
                MessageToSign = new MessageToSign()
                {
                    Message = messageToSign,
                    Nonce = nonce,
                }
            };
        }
    }
}
