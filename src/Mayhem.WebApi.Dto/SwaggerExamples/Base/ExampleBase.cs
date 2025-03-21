using Mayhem.Helper;
using Nethereum.Signer;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Mayhem.WebApi.Dto.SwaggerExamples.Base
{
    public abstract class ExampleBase<T> : IExamplesProvider<T>
    {
        public abstract T GetExamples();

        protected (string, string, long) GetSignature(string privateKey, string messageToSign)
        {
            EthereumMessageSigner signer = new();
            long nonce = DateTime.UtcNow.ToUnixTime();
            string signedMessage = signer.EncodeUTF8AndSign($"{messageToSign} {nonce}", new EthECKey(privateKey));

            return (signedMessage, messageToSign, nonce);
        }
    }
}
