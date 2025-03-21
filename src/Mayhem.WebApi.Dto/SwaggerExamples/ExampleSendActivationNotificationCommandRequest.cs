using Mayhem.Util.Classes;
using Mayhem.WebApi.Dto.SwaggerExamples.Base;
using Mayhen.Bl.Commands.SendActivationNotification;

namespace Mayhem.WebApi.Dto.SwaggerExamples
{
    public class ExampleSendActivationNotificationCommandRequest : ExampleBase<SendActivationNotificationCommandRequest>
    {
        private const string PrivateKey = "0xa302447dc83d1418360c91080791169f37ddaf1b6ced07315e687a596b0fc1ac";
        private const string Email = "pawel.spionkowski@adriagames.com";
        private const string Wallet = "0xe62e0ccc74cb7b4c7af31b096d72360c3c20b696";

        public override SendActivationNotificationCommandRequest GetExamples()
        {
            (string signedMessage, string messageToSign, long nonce) = GetSignature(PrivateKey, "test message");

            return new SendActivationNotificationCommandRequest()
            {
                Wallet = Wallet,
                Email = Email,
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
