using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.IntegrationTest.Base.Models;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.Register;
using Mayhen.Bl.Commands.SendActivationNotification;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Base
{
    public class BaseUserControllerTests : ControllerTestBase<BaseUserControllerTests>
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
    }
}
