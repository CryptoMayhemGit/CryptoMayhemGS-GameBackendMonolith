using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Helper;
using Mayhem.HttpClient.Interfaces;
using Mayhem.Util;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Signer;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Base
{
    public class ControllerTestBase<DBName>
    {
        private const string ExpectedBasePrivateKey = "0xa302447dc83d1418360c91080791169f37ddaf1b6ced07315e687a596b0fc1ac";
        protected const string ExpectedBaseEmail = "Base@wp.pl";
        protected const string ExpectedBaseWallet = "0xe62e0ccc74cb7b4c7af31b096d72360c3c20b696";

        protected const string ExpectedRegisterAndLoginPrivateKey = "de03878f9a92dd991f5711330d09c172ed14d608cac3881c5f629c67c68b82d6";
        protected const string ExpectedRegisterAndLoginEmail = "RegisterAndLogin@wp.pl";
        protected const string ExpectedRegisterAndLoginWallet = "0x018C7e30B12e3aBcd6208560E43ee825C5C76d33";

        private readonly CustomWebApplicationFactory _factory;

        protected readonly IHttpClientService httpClientService;
        protected string Token;
        protected int UserId => JwtSecurityTokenHelper.GetUserId(Token);

        public ControllerTestBase()
        {
            _factory = new CustomWebApplicationFactory(typeof(DBName).Name);
            System.Net.Http.HttpClient testHttpClient = _factory.CreateClient();

            httpClientService = GetService<IHttpClientService>();
            httpClientService.InjectTestHttpClient(testHttpClient);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            IMayhemDataContext mayhemDataContext = GetService<IMayhemDataContext>();
            IMayhemConfigurationService mayhemConfigurationService = GetService<IMayhemConfigurationService>();

            string sendActivationNotificationEnpoint = $"api/{ControllerNames.Account}/Activation";
            string registerEnpoint = $"api/{ControllerNames.Account}/Register";
            string loginEnpoint = $"api/{ControllerNames.Account}/Login";

            (string signedMessage, string messageToSign, long nonce) = GetSignature(ExpectedBasePrivateKey);

            SendActivationNotificationCommandRequest sendActivationNotificationRequest = new()
            {
                Wallet = ExpectedBaseWallet,
                Email = ExpectedBaseEmail,
                SignedMessage = signedMessage,
                MessageToSign = new MessageToSign()
                {
                    Message = messageToSign,
                    Nonce = nonce,
                }
            };

            await httpClientService.HttpPostAsJsonAsync<SendActivationNotificationCommandRequest, SendActivationNotificationCommandResponse>(sendActivationNotificationEnpoint, sendActivationNotificationRequest);

            Notification notification = await mayhemDataContext
                .Notifications
                .Where(x => x.WalletAddress.Equals(ExpectedBaseWallet)
                && x.Email == ExpectedBaseEmail)
                .SingleOrDefaultAsync();
            notification.WasSent = true;
            await mayhemDataContext.SaveChangesAsync();

            RegisterCommandRequest registeRequest = new()
            {
                ActivationNotificationToken = GetExpectedActivationNotificationToken(ExpectedBaseWallet, ExpectedBaseEmail),
            };

            await httpClientService.HttpPostAsJsonAsync<RegisterCommandRequest, RegisterCommandResponse>(registerEnpoint, registeRequest);

            LoginCommandRequest loginRequest = new()
            {
                Wallet = ExpectedBaseWallet,
                SignedMessage = signedMessage,
                MessageToSign = new MessageToSign()
                {
                    Message = messageToSign,
                    Nonce = nonce,
                }
            };

            ActionDataResult<LoginCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<LoginCommandRequest, LoginCommandResponse>(loginEnpoint, loginRequest);

            Token = response.Result.Token;
        }

        protected T GetService<T>()
        {
            return _factory.GetService<T>();
        }

        protected async Task<(int id, string token)> GetNewTokenAsync()
        {
            IUserRepository userRepository = GetService<IUserRepository>();
            IAuthService authService = GetService<IAuthService>();
            int? userId = await userRepository.CreateUserAsync(Guid.NewGuid().ToString(), ExpectedBaseEmail);
            string token = await authService.RefreshToken(userId.Value);
            return (userId.Value, token);
        }

        protected (int id, string token) GetFakeToken(int userId = 39204833)
        {
            IMayhemConfigurationService mayhemConfigurationService = GetService<IMayhemConfigurationService>();

            List<Claim> authClaims = new()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtKey));

            JwtSecurityToken jwtToken = new(
                issuer: mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtIssuer,
                audience: mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtAudience,
                expires: DateTime.UtcNow.AddMinutes(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.TokenLifetimeInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return (userId, token);
        }

        protected async Task<string> GetTokenByUserIdAsync(int userId)
        {
            IAuthService authService = GetService<IAuthService>();
            return await authService.RefreshToken(userId);
        }

        protected (string, string, long) GetSignature(string privateKey)
        {
            EthereumMessageSigner signer = new();
            long nonce = DateTime.UtcNow.ToUnixTime();
            string messageToSign = $"test message";
            string signedMessage = signer.EncodeUTF8AndSign($"{messageToSign} {nonce}", new EthECKey(privateKey));

            return (signedMessage, messageToSign, nonce);

        }
        protected string GetExpectedActivationNotificationToken(string wallet, string email)
        {
            NotificationDataDto notificationData = new()
            {
                Wallet = wallet,
                Email = email,
                CreationDate = DateTime.UtcNow
            };
            string serializeActivationNotificationData = JsonConvert.SerializeObject(notificationData);
            IMayhemConfigurationService mayhemConfigurationService = GetService<IMayhemConfigurationService>();
            return serializeActivationNotificationData.Encrypt(mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.ActivationTokenSecretKey);
        }

        protected (string address, string privateKey, string email) CreateWallet()
        {
            EthECKey key = EthECKey.GenerateKey();
            string privateKey = key.GetPrivateKey();
            string address = key.GetPublicAddress();
            return (address, privateKey, $"{Guid.NewGuid().ToString().Replace("-", "")}@adria.com");
        }
    }
}
