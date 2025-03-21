using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Helper;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Validators;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class RegisterCommandRequestValidatorTests : UnitTestBase
    {
        private IUserRepository userRepository;
        private IMayhemConfigurationService mayhemConfigurationService;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            userRepository = GetService<IUserRepository>();
            mayhemConfigurationService = GetService<IMayhemConfigurationService>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task CreateExistingUserAsync_WhenUserCreated_ThenThrowException_Test()
        {
            string expectedEmail = "test@o2.pl";
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");
            await userRepository.CreateUserAsync(walletAddress, expectedEmail);

            NotificationDataDto notificationData = new()
            {
                Email = expectedEmail,
                Wallet = walletAddress,
                CreationDate = DateTime.UtcNow,
            };

            string serializeActivationNotificationData = JsonConvert.SerializeObject(notificationData);
            string encrypted = serializeActivationNotificationData.Encrypt(mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.ActivationTokenSecretKey);

            RegisterCommandRequestValidator validator = new(mayhemConfigurationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new Mayhen.Bl.Commands.Register.RegisterCommandRequest()
            {
                ActivationNotificationToken = encrypted,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with wallet {walletAddress} already exists.");
            result.Errors.First().PropertyName.Should().Be($"WalletAddress");
        }
    }
}
