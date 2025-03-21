using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Seeds.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries
{
    public class ResourceTypeMappingConfiguration : DbTableBaseConfigurationMapping<ResourceType>
    {
        public override void Configure(EntityTypeBuilder<ResourceType> builder)
        {
            builder.ToTable(nameof(ResourceType), SchemaNames.DictionarySchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name);

            builder.HasData(EnumToSeedService.GetSeedData<ResourceType, ResourcesType>());

            base.Configure(builder);
        }
    }
}
