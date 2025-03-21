using AutoMapper;
using Mayhem.Dal.Dto.Classes.AuditLogs;
using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Util.Classes;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.SendActivationNotification
{
    public class SendActivationNotificationCommandHandler : IRequestHandler<SendActivationNotificationCommandRequest, SendActivationNotificationCommandResponse>
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly INotificationPublisherService notificationPublisherService;
        private readonly IMapper mapper;

        public SendActivationNotificationCommandHandler(INotificationRepository notificationRepository, IAuditLogRepository auditLogRepository, INotificationPublisherService notificationPublisherService, IMapper mapper)
        {
            this.notificationRepository = notificationRepository;
            this.auditLogRepository = auditLogRepository;
            this.notificationPublisherService = notificationPublisherService;
            this.mapper = mapper;
        }

        public async Task<SendActivationNotificationCommandResponse> Handle(SendActivationNotificationCommandRequest request, CancellationToken cancellationToken)
        {
            int notificationId = await notificationRepository.AddNotificationAsync(mapper.Map<SendActivationNotificationCommandRequestDto>(request));
            await notificationPublisherService.PublishMessageAsync(notificationId);

            await auditLogRepository.AddAuditLogAsync(mapper.Map<AuditLogDto>(request, opts =>
            {
                opts.Items[MappingParamConstants.NotificationActionParam] = AuditLogNames.Notification;
            }));

            return new SendActivationNotificationCommandResponse()
            {
                Success = true
            };
        }
    }
}
