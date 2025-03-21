using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Settings
{
    public class SettingMappingConfiguration : DbTableBaseConfigurationMapping<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable(nameof(Setting), SchemaNames.SettingSchema);

            builder.Property(e => e.Key);
            builder.Property(e => e.Value);

            base.Configure(builder);
        }
    }
}
