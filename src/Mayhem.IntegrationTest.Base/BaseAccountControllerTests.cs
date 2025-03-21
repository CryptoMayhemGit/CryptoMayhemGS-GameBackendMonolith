using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Helper;
using Mayhem.IntegrationTest.Base.Models;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.CheckAccount;
using Mayhen.Bl.Commands.CheckEmail;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Commands.SendActivationNotification;
using Mayhen.Bl.Commands.SendReativationNotification;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Base
{
    public class BaseAccountControllerTests : ControllerTestBase<BaseAccountControllerTests>
    {
        protected async Task<ActionDataResult<SendActivationNotificationCommandResponse>> SendActivationNotificationCommand(SendActivationNotificationTestDto sendActivationNotificationTestDto)
        {
            string sendActivationNotificationEnpoint = $"api/{ControllerNames.Account}/Activation";

            SendActivationNotificationCommandRequest sendActivationNotificationRequest = new()
            {
                Wallet = sendActivationNotificationTestDto.Wallet,
                Email = sendActivationNotificationTestDto.Email,
                SignedMessage = sendActivationNotificationTestDto.SignedMessage,
                MessageToSign = new MessageToSign()
                {
                    Message = sendActivationNotificationTestDto.MessageToSign,
                    Nonce = sendActivationNotificationTestDto.Nonce,
                }
            };

            ActionDataResult<SendActivationNotificationCommandResponse> result = await httpClientService.HttpPostAsJsonAsync<SendActivationNotificationCommandRequest, SendActivationNotificationCommandResponse>(sendActivationNotificationEnpoint, sendActivationNotificationRequest);

            IMayhemDataContext mayhemDataContext = GetService<IMayhemDataContext>();
            Notification notification = await mayhemDataContext
               .Notifications
               .Where(x => x.WalletAddress.Equals(sendActivationNotificationTestDto.Wallet)
               && x.Email == sendActivationNotificationTestDto.Email)
               .SingleOrDefaultAsync();

            if (notification == null)
            {
                return result;
            }

            notification.WasSent = true;
            await mayhemDataContext.SaveChangesAsync();

            return result;
        }

        protected async Task<ActionDataResult<SendReactivationNotificationCommandResponse>> SendReactivationNotificationCommand(SendReactivationNotificationCommandRequest request)
        {
            string endpoint = $"api/{ControllerNames.Account}/Reactivation";

            ActionDataResult<SendReactivationNotificationCommandResponse> result = await httpClientService.HttpPostAsJsonAsync<SendReactivationNotificationCommandRequest, SendReactivationNotificationCommandResponse>(endpoint, request);

            return result;
        }

        protected async Task<ActionDataResult<LoginCommandResponse>> LoginCommand(LoginCommandNotificationTestDto loginCommandNotificationTestDto, bool withDelay = false)
        {
            string loginEndpoint = $"api/{ControllerNames.Account}/Login";
            LoginCommandRequest loginRequest = new()
            {
                SignedMessage = loginCommandNotificationTestDto.SignedMessage,
                Wallet = loginCommandNotificationTestDto.Wallet,
                MessageToSign = new MessageToSign()
                {
                    Message = loginCommandNotificationTestDto.MessageToSign,
                    Nonce = (withDelay ? loginCommandNotificationTestDto.Nonce.FromUnixTime().AddMinutes(-30).ToUnixTime() : loginCommandNotificationTestDto.Nonce),
                }
            };

            ActionDataResult<LoginCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<LoginCommandRequest, LoginCommandResponse>(loginEndpoint, loginRequest);
            return response;
        }

        protected async Task<ActionDataResult<RegisterCommandResponse>> RegisterCommand(string activationNotificationToken)
        {
            string endpoint = $"api/{ControllerNames.Account}/Register";

            RegisterCommandRequest request = new()
            {
                ActivationNotificationToken = activationNotificationToken
            };

            ActionDataResult<RegisterCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<RegisterCommandRequest, RegisterCommandResponse>(endpoint, request);
            return response;
        }

        protected async Task<ActionDataResult<CheckEmailCommandResponse>> CheckEmailCommand(string email)
        {
            string endpoint = $"api/{ControllerNames.Account}/Verify/Email";

            CheckEmailCommandRequest request = new()
            {
                Email = email,
            };

            ActionDataResult<CheckEmailCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<CheckEmailCommandRequest, CheckEmailCommandResponse>(endpoint, request);
            return response;
        }

        protected async Task<ActionDataResult<Stream>> CheckAccountCommand(string walletAddress)
        {
            string endpoint = $"api/{ControllerNames.Account}/Check";

            CheckAccountCommandRequest request = new()
            {
                WalletAddress = walletAddress,
            };

            return await httpClientService.HttpPostAsStreamAsync(endpoint, request);
        }
    }
}
