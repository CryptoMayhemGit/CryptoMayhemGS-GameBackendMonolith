using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildResourceMappingConfiguration : DbTableBaseConfigurationMapping<GuildResource>
    {
        public override void Configure(EntityTypeBuilder<GuildResource> builder)
        {
            builder.ToTable(nameof(GuildResource), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.GuildId);
            builder.Property(e => e.ResourceTypeId);
            builder.Property(e => e.Value);

            builder.HasOne(x => x.ResourceType)
                .WithMany(y => y.GuildResources)
                .HasForeignKey(x => x.ResourceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Guild)
                .WithMany(y => y.GuildResources)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
