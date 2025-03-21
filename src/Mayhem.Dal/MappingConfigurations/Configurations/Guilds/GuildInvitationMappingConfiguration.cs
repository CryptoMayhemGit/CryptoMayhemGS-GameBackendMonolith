using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Guilds
{
    public class GuildInvitationMappingConfiguration : DbTableBaseConfigurationMapping<GuildInvitation>
    {
        public override void Configure(EntityTypeBuilder<GuildInvitation> builder)
        {
            builder.ToTable(nameof(GuildInvitation), SchemaNames.GuildSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.UserId);
            builder.Property(e => e.GuildId);

            builder.HasIndex(e => new { e.UserId, e.GuildId }).IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.GuildInvitations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Guild)
                .WithMany(x => x.GuildInvitations)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
