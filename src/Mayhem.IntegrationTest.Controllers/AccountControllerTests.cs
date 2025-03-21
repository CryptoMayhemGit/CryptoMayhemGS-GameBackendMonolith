using FluentAssertions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Classes.AuditLogs;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.IntegrationTest.Base;
using Mayhem.IntegrationTest.Base.Models;
using Mayhem.Util.Classes;
using Mayhen.Bl.Commands.CheckEmail;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Commands.SendReativationNotification;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class AccountControllerTests : BaseAccountControllerTests
    {
        private IMayhemDataContext mayhemDataContext;
        private IMayhemConfigurationService configurationService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            configurationService = GetService<IMayhemConfigurationService>();
        }

        [Test]
        public async Task RegisterNotExistingUser_WhenUserRegistered_ThenReturnSuccess_Test()
        {
            (string address, string privateKey, string email) = CreateWallet();
            (string signedMessage, string messageToSign, long nonce) = GetSignature(privateKey);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = address,
                Email = email
            };

            await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            ActionDataResult<RegisterCommandResponse> response = await RegisterCommand(GetExpectedActivationNotificationToken(address, email));

            AuditLog auditLog = await mayhemDataContext.AuditLogs.SingleOrDefaultAsync(x => x.Nonce == nonce);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Success.Should().BeTrue();
            response.Result.UserId.Should().BeGreaterThan(0);

            auditLog.Should().NotBeNull();
            auditLog.Nonce.Should().Be(nonce);
            auditLog.Message.Should().Be(messageToSign);
            auditLog.SignedMessage.Should().Be(signedMessage);
        }

        [Test]
        public async Task RegisterExistingUser_WhenUserRegistered_ThenReturnFailure_Test()
        {
            ActionDataResult<RegisterCommandResponse> response = await RegisterCommand(GetExpectedActivationNotificationToken(ExpectedBaseWallet, ExpectedBaseEmail));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"User with wallet {ExpectedBaseWallet} already exists.");
            response.Errors.First().FieldName.Should().Be("WalletAddress");
        }

        [Test, Order(3)]
        public async Task RegisterExistingUser_WhenTokenIsIncorrect_ThenReturnFailure_Test()
        {
            const string IncorrectToken = "IncorrectToken";
            ActionDataResult<RegisterCommandResponse> response = await RegisterCommand(IncorrectToken);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"The activation token is invalid or has expired.");
            response.Errors.First().FieldName.Should().Be("Token");
        }

        [Test]
        public async Task RegisterUser_WhenUserRegistered_ThenLoginAndGetToken_Test()
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

            await RegisterCommand(GetExpectedActivationNotificationToken(ExpectedRegisterAndLoginWallet, ExpectedRegisterAndLoginEmail));
            LoginCommandNotificationTestDto loginCommandNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = ExpectedRegisterAndLoginWallet,
            };
            ActionDataResult<LoginCommandResponse> response = await LoginCommand(loginCommandNotificationTestDto, false);

            List<AuditLog> auditLogs = await mayhemDataContext.AuditLogs.Where(x => x.Nonce == nonce).ToListAsync();

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Errors.Should().BeNull();
            response.Result.Should().NotBeNull();
            response.Result.Token.Should().NotBeNullOrEmpty();

            auditLogs.Should().HaveCount(2);

            auditLogs[0].Should().NotBeNull();
            auditLogs[0].Nonce.Should().Be(nonce);
            auditLogs[0].Message.Should().Be(messageToSign);
            auditLogs[0].SignedMessage.Should().Be(signedMessage);
            auditLogs[0].Action.Should().Be(AuditLogNames.Notification);

            auditLogs[1].Should().NotBeNull();
            auditLogs[1].Nonce.Should().Be(nonce);
            auditLogs[1].Message.Should().Be(messageToSign);
            auditLogs[1].SignedMessage.Should().Be(signedMessage);
            auditLogs[1].Action.Should().Be(AuditLogNames.Login);
        }

        [Test]
        public async Task RegisterUser_WhenOldNonce_ThenReturnFailure_Test()
        {
            await Task.Delay(1000);

            (string address, string privateKey, string email) = CreateWallet();

            (string signedMessage, string messageToSign, long nonce) = GetSignature(privateKey);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = address,
                Email = email
            };
            await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            await RegisterCommand(GetExpectedActivationNotificationToken(address, email));
            LoginCommandNotificationTestDto loginCommandNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = address,
            };
            ActionDataResult<LoginCommandResponse> response = await LoginCommand(loginCommandNotificationTestDto, true);

            List<AuditLog> auditLogs = await mayhemDataContext.AuditLogs.Where(x => x.Nonce == nonce).ToListAsync();

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Contain($"must be greater than");

            auditLogs.Should().HaveCount(1);

            auditLogs[0].Should().NotBeNull();
            auditLogs[0].Nonce.Should().Be(nonce);
            auditLogs[0].Message.Should().Be(messageToSign);
            auditLogs[0].SignedMessage.Should().Be(signedMessage);
            auditLogs[0].Action.Should().Be(AuditLogNames.Notification);
        }

        [Test]
        public async Task SendActivationNotification_WhenUserNotRegisteredAndNotificationNotExist_ThenReturnSuccess_Test()
        {
            await Task.Delay(1000);
            (string address, string privateKey, string email) = CreateWallet();
            (string signedMessage, string messageToSign, long nonce) = GetSignature(privateKey);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = address,
                Email = email
            };
            ActionDataResult<SendActivationNotificationCommandResponse> response = await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            AuditLog auditLogs = await mayhemDataContext.AuditLogs.SingleOrDefaultAsync(x => x.Nonce == nonce);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Success.Should().BeTrue();

            auditLogs.Should().NotBeNull();
            auditLogs.Nonce.Should().Be(nonce);
            auditLogs.Message.Should().Be(messageToSign);
            auditLogs.SignedMessage.Should().Be(signedMessage);
            auditLogs.Action.Should().Be(AuditLogNames.Notification);
        }

        [Test]
        public async Task SendActivationNotification_WhenUserRegistered_ThenReturnFailure_Test()
        {
            (string address, string privateKey, string email) = CreateWallet();
            (string signedMessage, string messageToSign, long nonce) = GetSignature(privateKey);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce,
                Wallet = address,
                Email = email
            };

            await SendActivationNotificationCommand(sendActivationNotificationTestDto);
            ActionDataResult<SendActivationNotificationCommandResponse> response = await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"Notification with wallet {address} already exists.");
        }

        [Test]
        public async Task CheckAccount_WhenUserNotRegistered_ThenReturnSuccess_Test()
        {
            string email = $"1BoatSLRHtKNngkdXEeobR76b53LETtpyT";

            ActionDataResult<Stream> response = await CheckAccountCommand(email);

            response.Errors.Should().BeNull();
        }

        [Test]
        public async Task SendActivationNotification_WhenUserRegisteredWithExpectedEmail_ThenReturnFailure_Test()
        {
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@adria.com";
            (string address1, string privateKey1, _) = CreateWallet();
            (string address2, string privateKey2, _) = CreateWallet();
            (string signedMessage1, string messageToSign1, long nonce1) = GetSignature(privateKey1);
            (string signedMessage2, string messageToSign2, long nonce2) = GetSignature(privateKey2);
            SendActivationNotificationTestDto sendActivationNotificationTestDto1 = new()
            {
                SignedMessage = signedMessage1,
                MessageToSign = messageToSign1,
                Nonce = nonce1,
                Wallet = address1,
                Email = email
            };
            SendActivationNotificationTestDto sendActivationNotificationTestDto2 = new()
            {
                SignedMessage = signedMessage2,
                MessageToSign = messageToSign2,
                Nonce = nonce2,
                Wallet = address2,
                Email = email
            };
            await SendActivationNotificationCommand(sendActivationNotificationTestDto1);
            ActionDataResult<SendActivationNotificationCommandResponse> response = await SendActivationNotificationCommand(sendActivationNotificationTestDto2);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"Notification with email {email} already exists.");
        }

        [Test]
        public async Task SendReactivationNotification_WhenUserActivationExist_ThenSend_Test()
        {
            int resendTime = configurationService.MayhemConfiguration.CommonConfiguration.ResendNotificationTimeInMinutes;
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@adria.com";
            (string address, string privateKey1, _) = CreateWallet();
            (string signedMessage, string messageToSign, long nonce1) = GetSignature(privateKey1);
            SendActivationNotificationTestDto sendActivationNotificationTestDto = new()
            {
                SignedMessage = signedMessage,
                MessageToSign = messageToSign,
                Nonce = nonce1,
                Wallet = address,
                Email = email
            };
            await SendActivationNotificationCommand(sendActivationNotificationTestDto);

            SendReactivationNotificationCommandRequest request = new()
            {
                Email = email,
            };
            await Task.Delay(resendTime * 60 * 1000);
            ActionDataResult<SendReactivationNotificationCommandResponse> result = await SendReactivationNotificationCommand(request);

            Notification notification = await mayhemDataContext.Notifications.SingleOrDefaultAsync(x => x.Email == email);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Errors.Should().BeNull();
            notification.Email.Should().Be(email);
        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsNotUsed_ThetReturnFalse_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";

            ActionDataResult<CheckEmailCommandResponse> response = await CheckEmailCommand(email);

            response.Result.Result.Should().BeFalse();
        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsInNotification_ThetReturnTrue_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";
            await mayhemDataContext.Notifications.AddAsync(new Notification()
            {
                Email = email,
            });

            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CheckEmailCommandResponse> response = await CheckEmailCommand(email);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsInUser_ThetReturnTrue_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";
            await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Email = email,
            });

            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CheckEmailCommandResponse> response = await CheckEmailCommand(email);

            response.Result.Result.Should().BeTrue();
        }
    }
}
