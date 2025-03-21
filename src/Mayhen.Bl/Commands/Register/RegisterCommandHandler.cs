using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Helper;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly ServiceSecretsConfigruation serviceSecretsConfigruation;

        public RegisterCommandHandler(IUserRepository userRepository,
                                      INotificationRepository notificationRepository,
                                      IMayhemConfigurationService mayhemConfigurationService)
        {
            serviceSecretsConfigruation = mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation;
            this.userRepository = userRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            NotificationDataDto notificationData = GetSerializeActivationNotificationData(request);

            await notificationRepository.CheckActivationLinkAsync(notificationData.Wallet, notificationData.Email);

            int? userId = await userRepository.CreateUserAsync(notificationData.Wallet, notificationData.Email);

            return new RegisterCommandResponse()
            {
                Success = userId.HasValue,
                UserId = userId
            };
        }

        private NotificationDataDto GetSerializeActivationNotificationData(RegisterCommandRequest request)
        {
            string serializeActivationNotificationData = request.ActivationNotificationToken.Decrypt(serviceSecretsConfigruation.ActivationTokenSecretKey);

            return JsonConvert.DeserializeObject<NotificationDataDto>(serializeActivationNotificationData);
        }
    }
}
