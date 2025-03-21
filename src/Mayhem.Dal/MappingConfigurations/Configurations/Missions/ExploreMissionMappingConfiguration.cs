using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Missions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Missions
{
    public class ExploreMissionMappingConfiguration : DbTableBaseConfigurationMapping<ExploreMission>
    {
        public override void Configure(EntityTypeBuilder<ExploreMission> builder)
        {
            builder.ToTable(nameof(ExploreMission), SchemaNames.MissionSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.NpcId);
            builder.Property(e => e.LandId);
            builder.Property(e => e.UserId);
            builder.Property(e => e.FinishDate);

            builder.HasIndex(e => e.NpcId).IsUnique();
            builder.HasIndex(e => new { e.NpcId, e.LandId });

            builder.HasOne(x => x.Npc)
               .WithOne(y => y.ExploreMission)
               .HasForeignKey<ExploreMission>(x => x.NpcId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
               .WithMany(y => y.ExploreMissions)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Land)
               .WithMany(y => y.ExploreMissions)
               .HasForeignKey(x => x.LandId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
