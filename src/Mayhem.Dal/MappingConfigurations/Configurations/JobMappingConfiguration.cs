using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class JobMappingConfiguration : DbTableBaseConfigurationMapping<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable(nameof(Job), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.LandId);
            builder.Property(e => e.NpcId);
            builder.Property(e => e.StartDate);

            builder.HasOne(x => x.Land)
               .WithMany(y => y.Jobs)
               .HasForeignKey(x => x.LandId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Npc)
               .WithOne(y => y.Job)
               .HasForeignKey<Job>(x => x.NpcId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
