using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Helper;
using Mayhem.UnitTest.Base;
using Mayhem.Util.Classes;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Validators;
using Nethereum.Signer;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class SendActivationNotificationCommandRequestValidatorTests : UnitTestBase
    {
        private const string Wallet1 = "0xe62e0ccc74cb7b4c7af31b096d72360c3c20b696";
        private const string PrivateKey1 = "0xa302447dc83d1418360c91080791169f37ddaf1b6ced07315e687a596b0fc1ac";

        private const string Wallet2 = "0x2871ab1B2Cd154deb5cf1bFe98D0CB09F0058E37";
        private const string PrivateKey2 = "2b4558851d9b611dd3549c2fdc03d122a00bfa64ea671f468246441ce9010ee8";

        private IUserRepository userRepository;
        private IMayhemConfigurationService mayhemConfigurationService;
        private IMayhemDataContext mayhemDataContext;
        private IBlockchainService blockchainService;

        [OneTimeSetUp]
        public void SetUp()
        {
            userRepository = GetService<IUserRepository>();
            mayhemConfigurationService = GetService<IMayhemConfigurationService>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            blockchainService = GetService<IBlockchainService>();
        }

        [Test, Order(1)]
        public async Task AddNotificationToQueueAsync_WhenGameUserCreated_ThenThrowExceptionWithInvalidWalletForGameUser_Test()
        {
            SendActivationNotificationCommandRequestDto sendActivationNotificationCommandRequestDto = new()
            {
                Wallet = Wallet1,
                Email = "GameUser@email.com"
            };

            await userRepository.CreateUserAsync(sendActivationNotificationCommandRequestDto.Wallet, "test@o2.pl");

            (string signedMessage, string messageToSign, long nonce) = GetSignature(PrivateKey1);

            SendActivationNotificationCommandRequestValidator validator = new(mayhemConfigurationService, blockchainService, mayhemDataContext);
            ValidationResult result = validator.Validate(new SendActivationNotificationCommandRequest()
            {
                Email = sendActivationNotificationCommandRequestDto.Email,
                Wallet = sendActivationNotificationCommandRequestDto.Wallet,
                MessageToSign = new MessageToSign()
                {
                    Message = messageToSign,
                    Nonce = nonce,
                },
                SignedMessage = signedMessage,
            });


            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with wallet {sendActivationNotificationCommandRequestDto.Wallet} already exists.");
            result.Errors.First().PropertyName.Should().Be($"WalletAddress");
        }

        [Test, Order(2)]
        public async Task AddNotificationToQueueAsync_WhenGameUserCreated_ThenThrowExceptionWithInvalidEmailForGameUser_Test()
        {
            SendActivationNotificationCommandRequestDto sendActivationNotificationCommandRequestDto = new()
            {
                Wallet = Wallet2,
                Email = "GameUser@email.com"
            };

            await userRepository.CreateUserAsync(Guid.NewGuid().ToString(), sendActivationNotificationCommandRequestDto.Email);

            (string signedMessage, string messageToSign, long nonce) = GetSignature(PrivateKey2);

            SendActivationNotificationCommandRequestValidator validator = new(mayhemConfigurationService, blockchainService, mayhemDataContext);
            ValidationResult result = validator.Validate(new SendActivationNotificationCommandRequest()
            {
                Email = sendActivationNotificationCommandRequestDto.Email,
                Wallet = sendActivationNotificationCommandRequestDto.Wallet,
                MessageToSign = new MessageToSign()
                {
                    Message = messageToSign,
                    Nonce = nonce,
                },
                SignedMessage = signedMessage,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with email {sendActivationNotificationCommandRequestDto.Email} already exists.");
            result.Errors.First().PropertyName.Should().Be($"EmailAddress");
        }

        private static (string, string, long) GetSignature(string privateKey)
        {
            EthereumMessageSigner signer = new();
            long nonce = DateTime.UtcNow.ToUnixTime();
            string messageToSign = "test message";
            string signedMessage = signer.EncodeUTF8AndSign($"{messageToSign} {nonce}", new EthECKey(privateKey));

            return (signedMessage, messageToSign, nonce);
        }
    }
}
