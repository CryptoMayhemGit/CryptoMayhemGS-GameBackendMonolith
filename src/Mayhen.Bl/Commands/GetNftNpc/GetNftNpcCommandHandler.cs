using AutoMapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Util.Classes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetNftNpc
{
    public class GetNftNpcCommandHandler : IRequestHandler<GetNftNpcCommandRequest, GetNftNpcCommandResponse>
    {
        private readonly INpcRepository npcRepository;
        private readonly IMapper mapper;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public GetNftNpcCommandHandler(INpcRepository npcRepository, IMapper mapper, IMayhemConfigurationService mayhemConfigurationService)
        {
            this.npcRepository = npcRepository;
            this.mapper = mapper;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<GetNftNpcCommandResponse> Handle(GetNftNpcCommandRequest request, CancellationToken cancellationToken)
        {
            NpcDto npc = await npcRepository.GetNpcByNftIdAsync(request.HeroNftId);

            if (npc == null || !npc.IsMinted)
            {
                return new GetNftNpcCommandResponse()
                {
                    Model = new NftStandardModel()
                    {
                        Image = mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint,
                        Name = "Hero",
                    },
                };
            }

            return new GetNftNpcCommandResponse()
            {
                Model = mapper.Map<NftStandardModel>(npc, opts =>
                {
                    opts.Items[MappingParamConstants.DescNpcParam] = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftNpcDescription;
                    opts.Items[MappingParamConstants.UrlNpcParam] = $"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{npc.Address}";
                }),
            };
        }
    }
}
