using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class NotificationMappingConfiguration : DbTableBaseConfigurationMapping<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.WalletAddress).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.WasSent);

            builder.HasIndex(x => x.WalletAddress).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();

            base.Configure(builder);
        }
    }
}
