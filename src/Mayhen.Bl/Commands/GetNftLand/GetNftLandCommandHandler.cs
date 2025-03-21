using AutoMapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Util.Classes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetNftLand
{
    public class GetNftLandCommandHandler : IRequestHandler<GetNftLandCommandRequest, GetNftLandCommandResponse>
    {
        private readonly ILandRepository landRepository;
        private readonly IMapper mapper;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public GetNftLandCommandHandler(ILandRepository landRepository, IMapper mapper, IMayhemConfigurationService mayhemConfigurationService)
        {
            this.landRepository = landRepository;
            this.mapper = mapper;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<GetNftLandCommandResponse> Handle(GetNftLandCommandRequest request, CancellationToken cancellationToken)
        {
            LandDto land = await landRepository.GetLandByNftIdAsync(request.LandNftId);

            if (land == null || !land.IsMinted)
            {
                return new GetNftLandCommandResponse()
                {
                    Model = new NftStandardModel()
                    {
                        Image = mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint,
                        Name = "Land",
                    },
                };
            }

            return new GetNftLandCommandResponse()
            {
                Model = mapper.Map<NftStandardModel>(land, opts =>
                {
                    opts.Items[MappingParamConstants.DescLandParam] = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftLandDescription;
                    opts.Items[MappingParamConstants.UrlLandParam] = $"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{land.Address}";
                }),
            };
        }
    }
}
