using AutoMapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Util.Classes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetNftItem
{
    public class GetNftItemCommandHandler : IRequestHandler<GetNftItemCommandRequest, GetNftItemCommandResponse>
    {
        private readonly IItemRepository itemRepository;
        private readonly IMapper mapper;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public GetNftItemCommandHandler(IItemRepository itemRepository, IMapper mapper, IMayhemConfigurationService mayhemConfigurationService)
        {
            this.itemRepository = itemRepository;
            this.mapper = mapper;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<GetNftItemCommandResponse> Handle(GetNftItemCommandRequest request, CancellationToken cancellationToken)
        {
            ItemDto item = await itemRepository.GetItemByNftIdAsync(request.ItemNftId);

            if (item == null || !item.IsMinted)
            {
                return new GetNftItemCommandResponse()
                {
                    Model = new NftStandardModel()
                    {
                        Image = mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint,
                        Name = "Item",
                    },
                };
            }

            return new GetNftItemCommandResponse()
            {
                Model = mapper.Map<NftStandardModel>(item, opts =>
                {
                    opts.Items[MappingParamConstants.DescItemParam] = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftItemDescription;
                    opts.Items[MappingParamConstants.UrlItemParam] = $"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{item.Address}";
                }),
            };
        }
    }
}
