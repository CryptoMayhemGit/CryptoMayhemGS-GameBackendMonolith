using AutoMapper;
using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Util.Classes;
using Mayhen.Bl.Commands.AddGuildImprovement;
using Mayhen.Bl.Commands.AddImprovement;
using Mayhen.Bl.Commands.GetUser;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Commands.SendActivationNotification;

namespace Mayhen.Bl.Mappings.Commands
{
    public class CommandsMappings : Profile
    {
        public CommandsMappings()
        {
            CreateMap<AddImprovementCommandRequest, ImprovementDto>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreationDate, y => y.Ignore())
                .ForMember(x => x.LastModificationDate, y => y.Ignore());

            CreateMap<GetUserCommandResponse, GetUserCommandResponseDto>().ReverseMap();
            CreateMap<GetUserCommandRequest, GetUserCommandRequestDto>().ReverseMap();

            CreateMap<SendActivationNotificationCommandRequest, SendActivationNotificationCommandRequestDto>();

            CreateMap<GuildImprovementDto, AddGuildImprovementCommandRequest>()
                .ForMember(x => x.UserId, y => y.Ignore());
            CreateMap<AddGuildImprovementCommandRequest, GuildImprovementDto>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreationDate, y => y.Ignore())
                .ForMember(x => x.LastModificationDate, y => y.Ignore());

            CreateMap<LoginCommandRequest, AuditLogDto>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreationDate, y => y.Ignore())
                .ForMember(x => x.LastModificationDate, y => y.Ignore())
                .ForMember(x => x.Wallet, y => y.MapFrom(z => z.Wallet))
                .ForMember(x => x.Action, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.LoginActionParam]))
                .ForMember(x => x.Message, y => y.MapFrom(z => z.MessageToSign.Message))
                .ForMember(x => x.Nonce, y => y.MapFrom(z => z.MessageToSign.Nonce));

            CreateMap<SendActivationNotificationCommandRequest, AuditLogDto>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreationDate, y => y.Ignore())
                .ForMember(x => x.LastModificationDate, y => y.Ignore())
                .ForMember(x => x.Wallet, y => y.MapFrom(z => z.Wallet))
                .ForMember(x => x.Action, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.NotificationActionParam]))
                .ForMember(x => x.Message, y => y.MapFrom(z => z.MessageToSign.Message))
                .ForMember(x => x.Nonce, y => y.MapFrom(z => z.MessageToSign.Nonce));
        }
    }
}
