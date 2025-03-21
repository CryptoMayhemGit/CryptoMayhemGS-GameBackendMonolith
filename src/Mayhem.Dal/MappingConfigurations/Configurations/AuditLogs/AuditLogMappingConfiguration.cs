using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.AuditLogs
{
    public class AuditLogMappingConfiguration : DbTableBaseConfigurationMapping<AuditLog>
    {
        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable(nameof(AuditLog), SchemaNames.LogSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Action).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Wallet).IsRequired().HasMaxLength(200);
            builder.Property(e => e.SignedMessage).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.Nonce);

            base.Configure(builder);
        }
    }
}
