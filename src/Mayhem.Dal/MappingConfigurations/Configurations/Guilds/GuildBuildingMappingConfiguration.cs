using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildBuildingMappingConfiguration : DbTableBaseConfigurationMapping<GuildBuilding>
    {
        public override void Configure(EntityTypeBuilder<GuildBuilding> builder)
        {
            builder.ToTable(nameof(GuildBuilding), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Level).IsRequired();
            builder.Property(e => e.GuildId).IsRequired();

            builder.HasIndex(e => e.GuildId);

            builder.HasOne(x => x.Guild)
                .WithMany(x => x.GuildBuildings)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GuildBuildingType)
                .WithMany(x => x.GuildBuildings)
                .HasForeignKey(x => x.GuildBuildingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
