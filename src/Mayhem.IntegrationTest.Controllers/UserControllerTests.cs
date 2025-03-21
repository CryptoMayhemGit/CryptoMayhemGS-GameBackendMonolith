using FluentAssertions;
using Mayhem.Dal.Dto.Classes.AuditLogs;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.Helper;
using Mayhem.IntegrationTest.Base;
using Mayhem.IntegrationTest.Base.Models;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.GetUser;
using Mayhen.Bl.Commands.Register;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class UserControllerTests : BaseUserControllerTests
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task RegisterUser_WhenUserRegistered_ThenGetIt_Test()
        {
            await Task.Delay(1000);

            (string signedMessage, string messageToSign, long nonce) = GetSignature(ExpectedRegisterAndLoginPrivateKey);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = ExpectedRegisterAndLoginWallet,
                Email = ExpectedRegisterAndLoginEmail
            };
            await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            ActionDataResult<RegisterCommandResponse> registerResult = await RegisterCommand(GetExpectedActivationNotificationToken(ExpectedRegisterAndLoginWallet, ExpectedRegisterAndLoginEmail));

            List<AuditLog> auditLogs = await mayhemDataContext.AuditLogs.Where(x => x.Nonce == nonce).ToListAsync();

            GetUserCommandRequest getUserRequest = new()
            {
                WithResources = true,
            };

            string getUserEndpoint = $"api/{ControllerNames.User}?{getUserRequest.ToQueryString()}";

            string newToken = await GetTokenByUserIdAsync(registerResult.Result.UserId.Value);

            ActionDataResult<GetUserCommandResponse> userResponse = await httpClientService.HttpGetAsJsonAsync<GetUserCommandResponse>(getUserEndpoint, newToken);

            userResponse.IsSuccessStatusCode.Should().BeTrue();
            userResponse.Errors.Should().BeNull();
            userResponse.Result.GameUser.Id.Should().Be(registerResult.Result.UserId.Value);
            userResponse.Result.UserResources.Should().HaveCount(7);

            auditLogs.Should().HaveCount(1);

            auditLogs[0].Should().NotBeNull();
            auditLogs[0].Nonce.Should().Be(nonce);
            auditLogs[0].Message.Should().Be(messageToSign);
            auditLogs[0].SignedMessage.Should().Be(signedMessage);
            auditLogs[0].Action.Should().Be(AuditLogNames.Notification);
        }
    }
}
