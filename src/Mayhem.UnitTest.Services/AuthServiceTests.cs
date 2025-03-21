using FluentAssertions;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Services
{
    public class AuthServiceTests : UnitTestBase
    {
        private IAuthService authService;
        private IUserRepository userRepository;
        protected const string ExpectedEmail = "testEmail@wp.pl";

        [OneTimeSetUp]
        public void Setup()
        {
            authService = GetService<IAuthService>();
            userRepository = GetService<IUserRepository>();
        }

        [Test]
        public async Task RegisterUser_WhenUserRegistered_ThenGetToken_Test()
        {
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");

            await userRepository.CreateUserAsync(walletAddress, ExpectedEmail);

            string token = await authService.CreateTokenAsync(walletAddress);

            token.Should().NotBeEmpty();
            token.Should().StartWith("ey");
        }

        [Test]
        public async Task GetToken_WhenUserNotRegister_ThenReturnEmptyString_Test()
        {
            string token = await authService.CreateTokenAsync(Guid.NewGuid().ToString().Replace("-", ""));

            token.Should().BeEmpty();
        }

        [Test]
        public async Task RefreshToken_WhenTokenRefreshed_ThenGetIt_Test()
        {
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");

            int? userId = await userRepository.CreateUserAsync(walletAddress, ExpectedEmail);

            string token = await authService.RefreshToken(userId.Value);

            token.Should().NotBeEmpty();
        }
    }
}
