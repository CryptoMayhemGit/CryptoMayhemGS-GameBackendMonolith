using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Buildings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Buildings
{
    public class BuildingBonusMappingConfiguration : DbTableBaseConfigurationMapping<BuildingBonus>
    {
        public override void Configure(EntityTypeBuilder<BuildingBonus> builder)
        {
            builder.ToTable(nameof(BuildingBonus), SchemaNames.BuildingSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.BuildingId).IsRequired();
            builder.Property(e => e.BuildingBonusTypeId).IsRequired();
            builder.Property(e => e.Bonus).IsRequired();

            builder.HasOne(x => x.BuildingBonusType)
                .WithOne(x => x.BuildingBonus)
                .HasForeignKey<BuildingBonus>(x => x.BuildingBonusTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
