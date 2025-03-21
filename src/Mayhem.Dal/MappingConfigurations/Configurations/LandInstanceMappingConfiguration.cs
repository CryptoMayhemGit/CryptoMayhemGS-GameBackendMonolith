using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class LandInstanceMappingConfiguration : DbTableBaseConfigurationMapping<LandInstance>
    {
        public override void Configure(EntityTypeBuilder<LandInstance> builder)
        {
            builder.ToTable(nameof(LandInstance), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);

            base.Configure(builder);
        }
    }
}
