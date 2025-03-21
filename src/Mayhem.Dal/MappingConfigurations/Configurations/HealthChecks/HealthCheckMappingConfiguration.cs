using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.HealthChecks
{
    public class HealthCheckMappingConfiguration : DbTableBaseConfigurationMapping<HealthCheck>
    {
        public override void Configure(EntityTypeBuilder<HealthCheck> builder)
        {
            builder.ToTable(nameof(HealthCheck), SchemaNames.HealthCheckSchema);

            builder.Property(e => e.Id);

            base.Configure(builder);
        }
    }
}
