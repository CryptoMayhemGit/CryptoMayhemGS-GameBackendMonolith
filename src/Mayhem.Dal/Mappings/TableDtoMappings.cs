using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;

namespace Mayhem.Dal.Mappings
{
    public class TableDtoMappings : Profile
    {
        public TableDtoMappings()
        {
            CreateMap<GameUser, GameUserDto>();
            CreateMap<GameUserDto, GameUser>()
                .ForMember(x => x.UserResources, y => y.Ignore())
                .ForMember(x => x.Items, y => y.Ignore())
                .ForMember(x => x.Npcs, y => y.Ignore())
                .ForMember(x => x.GuildMember, y => y.Ignore())
                .ForMember(x => x.GuildOwner, y => y.Ignore())
                .ForMember(x => x.UserLands, y => y.Ignore())
                .ForMember(x => x.ExploreMissions, y => y.Ignore())
                .ForMember(x => x.DiscoveryMissions, y => y.Ignore())
                .ForMember(x => x.GuildInvitations, y => y.Ignore());

            CreateMap<UserResource, UserResourceDto>();
            CreateMap<UserResourceDto, UserResource>()
                .ForMember(x => x.ResourceType, y => y.Ignore())
                .ForMember(x => x.GameUser, y => y.Ignore());

            CreateMap<Npc, NpcDto>();
            CreateMap<NpcDto, Npc>()
                .ForMember(x => x.Building, y => y.Ignore())
                .ForMember(x => x.GameUser, y => y.Ignore())
                .ForMember(x => x.Item, y => y.Ignore())
                .ForMember(x => x.Job, y => y.Ignore())
                .ForMember(x => x.Travels, y => y.Ignore())
                .ForMember(x => x.Land, y => y.Ignore())
                .ForMember(x => x.ExploreMission, y => y.Ignore())
                .ForMember(x => x.DiscoveryMission, y => y.Ignore())
                .ForMember(x => x.NpcStatus, y => y.Ignore())
                .ForMember(x => x.NpcType, y => y.Ignore());

            CreateMap<Attribute, AttributeDto>();
            CreateMap<AttributeDto, Attribute>()
                .ForMember(x => x.Npc, y => y.Ignore())
                .ForMember(x => x.AttributeType, y => y.Ignore());


            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>()
                .ForMember(x => x.GameUser, y => y.Ignore())
                .ForMember(x => x.Npc, y => y.Ignore())
                .ForMember(x => x.ItemType, y => y.Ignore());

            CreateMap<ItemBonus, ItemBonusDto>();
            CreateMap<ItemBonusDto, ItemBonus>()
                .ForMember(x => x.Item, y => y.Ignore())
                .ForMember(x => x.ItemBonusType, y => y.Ignore());

            CreateMap<Land, LandDto>();
            CreateMap<LandDto, Land>()
                .ForMember(x => x.LandInstance, y => y.Ignore())
                .ForMember(x => x.Improvements, y => y.Ignore())
                .ForMember(x => x.LandType, y => y.Ignore())
                .ForMember(x => x.Npcs, y => y.Ignore())
                .ForMember(x => x.TravelsFrom, y => y.Ignore())
                .ForMember(x => x.TravelsTo, y => y.Ignore())
                .ForMember(x => x.Jobs, y => y.Ignore())
                .ForMember(x => x.DiscoveryMissions, y => y.Ignore())
                .ForMember(x => x.ExploreMissions, y => y.Ignore())
                .ForMember(x => x.Buildings, y => y.Ignore());

            CreateMap<Land, SimpleLandDto>();
            CreateMap<SimpleLandDto, LandDto>()
                .ForMember(x => x.Address, y => y.Ignore())
                .ForMember(x => x.LandInstanceId, y => y.Ignore())
                .ForMember(x => x.LandTypeId, y => y.Ignore())
                .ForMember(x => x.Name, y => y.Ignore())
                .ForMember(x => x.IsMinted, y => y.Ignore())
                .ForMember(x => x.CreationDate, y => y.Ignore())
                .ForMember(x => x.LastModificationDate, y => y.Ignore())
                .ForMember(x => x.UserLands, y => y.Ignore());

            CreateMap<UserLand, UserLandDto>();
            CreateMap<UserLandDto, UserLand>()
                .ForMember(x => x.User, y => y.Ignore());

            CreateMap<Land, UndiscoveredLandDto>();
            CreateMap<Improvement, ImprovementDto>();
            CreateMap<ImprovementDto, Improvement>()
                .ForMember(x => x.ImprovementType, y => y.Ignore())
                .ForMember(x => x.Land, y => y.Ignore());

            CreateMap<Job, JobDto>();
            CreateMap<JobDto, Job>()
                .ForMember(x => x.Land, y => y.Ignore())
                .ForMember(x => x.Npc, y => y.Ignore());

            CreateMap<Travel, TravelDto>();
            CreateMap<TravelDto, Travel>()
                .ForMember(x => x.Npc, y => y.Ignore())
                .ForMember(x => x.LandFrom, y => y.Ignore())
                .ForMember(x => x.LandTo, y => y.Ignore());

            CreateMap<Building, BuildingDto>();
            CreateMap<BuildingDto, Building>()
                .ForMember(x => x.Land, y => y.Ignore())
                .ForMember(x => x.BuildingType, y => y.Ignore())
                .ForMember(x => x.BuildingBonuses, y => y.Ignore())
                .ForMember(x => x.Npcs, y => y.Ignore());

            CreateMap<BuildingBonus, BuildingBonusDto>();
            CreateMap<BuildingBonusDto, BuildingBonus>()
                .ForMember(x => x.Building, y => y.Ignore())
                .ForMember(x => x.BuildingBonusType, y => y.Ignore());

            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();

            CreateMap<Guild, GuildDto>();
            CreateMap<GuildDto, Guild>()
                .ForMember(x => x.GuildInvitations, y => y.Ignore())
                .ForMember(x => x.GuildBuildings, y => y.Ignore())
                .ForMember(x => x.GuildImprovements, y => y.Ignore())
                .ForMember(x => x.Owner, y => y.Ignore());

            CreateMap<GuildResource, GuildResourceDto>();
            CreateMap<GuildResourceDto, GuildResource>()
                .ForMember(x => x.ResourceType, y => y.Ignore())
                .ForMember(x => x.Guild, y => y.Ignore());

            CreateMap<GuildBuilding, GuildBuildingDto>();

            CreateMap<GuildBuildingBonus, GuildBuildingBonusDto>();

            CreateMap<GuildInvitation, GuildInvitationDto>().ReverseMap();

            CreateMap<GuildImprovement, GuildImprovementDto>();
            CreateMap<GuildImprovementDto, GuildImprovement>()
                .ForMember(x => x.Guild, y => y.Ignore())
                .ForMember(x => x.GuildImprovementType, y => y.Ignore());

            CreateMap<AuditLog, AuditLogDto>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();

            CreateMap<ExploreMission, ExploreMissionDto>();
            CreateMap<ExploreMissionDto, ExploreMission>()
                .ForMember(x => x.Land, y => y.Ignore())
                .ForMember(x => x.User, y => y.Ignore())
                .ForMember(x => x.Npc, y => y.Ignore());

            CreateMap<DiscoveryMission, DiscoveryMissionDto>();
            CreateMap<DiscoveryMissionDto, DiscoveryMission>()
                .ForMember(x => x.Land, y => y.Ignore())
                .ForMember(x => x.User, y => y.Ignore())
                .ForMember(x => x.Npc, y => y.Ignore());
        }
    }
}