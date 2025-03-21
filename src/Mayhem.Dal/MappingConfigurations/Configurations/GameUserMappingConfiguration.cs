using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class GameUserMappingConfiguration : DbTableBaseConfigurationMapping<GameUser>
    {
        public override void Configure(EntityTypeBuilder<GameUser> builder)
        {
            builder.ToTable(nameof(GameUser), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.WalletAddress).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(320);
            builder.Property(e => e.LastLoginDate);
            builder.Property(e => e.GuildId);

            builder.HasIndex(x => x.WalletAddress).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasOne(x => x.GuildMember)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
