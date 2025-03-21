using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Missions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Missions
{
    public class DiscoveryMissionMappingConfiguration : DbTableBaseConfigurationMapping<DiscoveryMission>
    {
        public override void Configure(EntityTypeBuilder<DiscoveryMission> builder)
        {
            builder.ToTable(nameof(DiscoveryMission), SchemaNames.MissionSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.NpcId);
            builder.Property(e => e.LandId);
            builder.Property(e => e.UserId);
            builder.Property(e => e.FinishDate);

            builder.HasIndex(e => e.NpcId).IsUnique();
            builder.HasIndex(e => new { e.NpcId, e.LandId });

            builder.HasOne(x => x.Npc)
               .WithOne(y => y.DiscoveryMission)
               .HasForeignKey<DiscoveryMission>(x => x.NpcId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
               .WithMany(y => y.DiscoveryMissions)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Land)
               .WithMany(y => y.DiscoveryMissions)
               .HasForeignKey(x => x.LandId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
