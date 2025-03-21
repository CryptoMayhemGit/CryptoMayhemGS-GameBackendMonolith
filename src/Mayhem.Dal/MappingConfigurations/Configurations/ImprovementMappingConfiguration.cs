using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class ImprovementMappingConfiguration : DbTableBaseConfigurationMapping<Improvement>
    {
        public override void Configure(EntityTypeBuilder<Improvement> builder)
        {
            builder.ToTable(nameof(Improvement), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.LandId);
            builder.Property(e => e.ImprovementTypeId);

            builder.HasIndex(x => x.LandId);

            builder.HasOne(x => x.Land)
                .WithMany(x => x.Improvements)
                .HasForeignKey(x => x.LandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ImprovementType)
                .WithMany(y => y.Improvements)
                .HasForeignKey(x => x.ImprovementTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
