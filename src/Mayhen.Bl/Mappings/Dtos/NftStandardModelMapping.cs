using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Util.Classes;

namespace Mayhen.Bl.Mappings.Dtos
{
    public class NftStandardModelMapping : Profile
    {
        public NftStandardModelMapping()
        {
            CreateMap<ItemDto, NftStandardModel>()
                .ForMember(x => x.Description, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.DescItemParam]))
                .ForMember(x => x.Image, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.UrlItemParam]))
                .ForMember(x => x.ExternalUrl, y => y.Ignore());

            CreateMap<LandDto, NftStandardModel>()
                .ForMember(x => x.Description, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.DescLandParam]))
                .ForMember(x => x.Image, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.UrlLandParam]))
                .ForMember(x => x.ExternalUrl, y => y.Ignore());

            CreateMap<NpcDto, NftStandardModel>()
                .ForMember(x => x.Description, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.DescNpcParam]))
                .ForMember(x => x.Image, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.UrlNpcParam]))
                .ForMember(x => x.ExternalUrl, y => y.Ignore());
        }
    }
}
