using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildBuildingBonusMappingConfiguration : DbTableBaseConfigurationMapping<GuildBuildingBonus>
    {
        public override void Configure(EntityTypeBuilder<GuildBuildingBonus> builder)
        {
            builder.ToTable(nameof(GuildBuildingBonus), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.GuildBuildingId).IsRequired();
            builder.Property(e => e.GuildBuildingBonusTypeId).IsRequired();
            builder.Property(e => e.Bonus).IsRequired();

            builder.HasOne(x => x.GuildBuildingBonusType)
                .WithOne(x => x.GuildBuildingBonus)
                .HasForeignKey<GuildBuildingBonus>(x => x.GuildBuildingBonusTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GuildBuilding)
                .WithMany(x => x.GuildBuildingBonuses)
                .HasForeignKey(x => x.GuildBuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
