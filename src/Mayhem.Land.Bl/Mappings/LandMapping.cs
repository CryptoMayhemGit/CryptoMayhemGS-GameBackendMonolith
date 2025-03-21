using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Land.Bl.Dtos;
using Mayhem.Util.Classes;

namespace Mayhem.Land.Bl.Mappings
{
    public class LandMapping : Profile
    {
        public LandMapping()
        {
            CreateMap<ImportLandDto, LandDto>()
                .ForMember(x => x.LandInstanceId, y => y.MapFrom((s, d, m, context) => context.Items[MappingParamConstants.LandInstanceParam]))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.PositionX, y => y.MapFrom(s => s.X))
                .ForMember(x => x.PositionY, y => y.MapFrom(s => s.Y))
                .ForMember(x => x.LandTypeId, y => y.MapFrom(s => s.Type))
                .ForMember(x => x.Name, y => y.Ignore())
                .ForMember(x => x.Address, y => y.Ignore())
                .ForMember(x => x.IsMinted, y => y.Ignore())
                .ForMember(x => x.UserLands, y => y.Ignore());
        }
    }
}
