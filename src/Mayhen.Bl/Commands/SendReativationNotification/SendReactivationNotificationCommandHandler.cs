using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.SendReativationNotification
{
    public class SendReactivationNotificationCommandHandler : IRequestHandler<SendReactivationNotificationCommandRequest, SendReactivationNotificationCommandResponse>
    {
        private readonly INotificationRepository notificationRepository;
        private readonly INotificationPublisherService notificationPublisherService;

        public SendReactivationNotificationCommandHandler(
            INotificationRepository notificationRepository,
            INotificationPublisherService notificationPublisherService)
        {
            this.notificationRepository = notificationRepository;
            this.notificationPublisherService = notificationPublisherService;
        }

        public async Task<SendReactivationNotificationCommandResponse> Handle(SendReactivationNotificationCommandRequest request, CancellationToken cancellationToken)
        {
            NotificationDto notificationDto = await notificationRepository.GetNotificationByEmailAsync(request.Email);
            await notificationPublisherService.PublishMessageAsync(notificationDto.Id);
            await notificationRepository.UpdateNotificationDate(notificationDto.Id);

            return new SendReactivationNotificationCommandResponse()
            {
                Success = true
            };
        }
    }
}
