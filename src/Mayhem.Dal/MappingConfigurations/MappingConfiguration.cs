using Mayhem.Dal.MappingConfigurations.Configurations;
using Mayhem.Dal.MappingConfigurations.Configurations.AuditLogs;
using Mayhem.Dal.MappingConfigurations.Configurations.Buildings;
using Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Guilds;
using Mayhem.Dal.MappingConfigurations.Configurations.HealthChecks;
using Mayhem.Dal.MappingConfigurations.Configurations.Missions;
using Mayhem.Dal.MappingConfigurations.Configurations.Nfts;
using Mayhem.Dal.MappingConfigurations.Configurations.Settings;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.MappingConfigurations
{
    /// <summary>
    /// Creates sql tables by tables configurations    
    /// </summary>
    public static class MappingConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuildingBonusMappingConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingMappingConfiguration());

            modelBuilder.ApplyConfiguration(new AttributeTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new BlockTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingBonusTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildBuildingBonusTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildBuildingTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildImprovementTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ImprovementTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ItemBonusTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new NpcTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new LandTypeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new NpcsStatusMappingConfiguration());

            modelBuilder.ApplyConfiguration(new GuildBuildingBonusMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildBuildingMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildImprovementMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildInvitationMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GuildResourceMappingConfiguration());

            modelBuilder.ApplyConfiguration(new HealthCheckMappingConfiguration());

            modelBuilder.ApplyConfiguration(new AuditLogMappingConfiguration());

            modelBuilder.ApplyConfiguration(new ItemMappingConfiguration());
            modelBuilder.ApplyConfiguration(new LandMappingConfiguration());
            modelBuilder.ApplyConfiguration(new NpcMappingConfiguration());

            modelBuilder.ApplyConfiguration(new ApplicationUserMappingConfiguration());
            modelBuilder.ApplyConfiguration(new AttributeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new BlockMappingConfiguration());
            modelBuilder.ApplyConfiguration(new GameUserMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ImprovementMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ItemBonusMappingConfiguration());
            modelBuilder.ApplyConfiguration(new JobMappingConfiguration());
            modelBuilder.ApplyConfiguration(new LandInstanceMappingConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationMappingConfiguration());
            modelBuilder.ApplyConfiguration(new TravelMappingConfiguration());
            modelBuilder.ApplyConfiguration(new UserLandMappingConfiguration());
            modelBuilder.ApplyConfiguration(new UserResourceMappingConfiguration());

            modelBuilder.ApplyConfiguration(new ExploreMissionMappingConfiguration());
            modelBuilder.ApplyConfiguration(new DiscoveryMissionMappingConfiguration()); 

            modelBuilder.ApplyConfiguration(new SettingMappingConfiguration()); 
        }
    }
}
