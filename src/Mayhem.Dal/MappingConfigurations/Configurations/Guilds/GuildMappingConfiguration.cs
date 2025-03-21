using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildMappingConfiguration : DbTableBaseConfigurationMapping<Guild>
    {
        public override void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.ToTable(nameof(Guild), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(800);
            builder.Property(e => e.OwnerId);

            builder.HasIndex(e => e.Name).IsUnique();

            builder.HasOne(x => x.Owner)
                .WithOne(x => x.GuildOwner)
                .HasForeignKey<Guild>(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
