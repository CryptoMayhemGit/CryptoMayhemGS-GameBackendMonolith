using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Buildings
{
    public class BuildingMappingConfiguration : DbTableBaseConfigurationMapping<Building>
    {
        public override void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToTable(nameof(Building), SchemaNames.BuildingSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.LandId);
            builder.Property(e => e.Level).IsRequired();

            builder.HasMany(x => x.BuildingBonuses)
                .WithOne(x => x.Building)
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BuildingType)
                .WithMany(x => x.Buildings)
                .HasForeignKey(x => x.BuildingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
