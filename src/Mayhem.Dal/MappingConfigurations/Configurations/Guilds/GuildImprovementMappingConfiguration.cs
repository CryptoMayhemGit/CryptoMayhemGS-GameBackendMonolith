using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildImprovementMappingConfiguration : DbTableBaseConfigurationMapping<GuildImprovement>
    {
        public override void Configure(EntityTypeBuilder<GuildImprovement> builder)
        {
            builder.ToTable(nameof(GuildImprovement), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.GuildId);
            builder.Property(e => e.GuildImprovementTypeId);

            builder.HasOne(x => x.Guild)
                .WithMany(x => x.GuildImprovements)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GuildImprovementType)
                .WithMany(y => y.GuildImprovements)
                .HasForeignKey(x => x.GuildImprovementTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
