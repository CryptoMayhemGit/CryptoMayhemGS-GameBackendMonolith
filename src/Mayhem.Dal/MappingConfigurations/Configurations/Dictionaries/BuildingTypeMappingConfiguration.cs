using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Seeds.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries
{
    public class BuildingTypeMappingConfiguration : DbTableBaseConfigurationMapping<BuildingType>
    {
        public override void Configure(EntityTypeBuilder<BuildingType> builder)
        {
            builder.ToTable(nameof(BuildingType), SchemaNames.DictionarySchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name);

            builder.HasData(EnumToSeedService.GetSeedData<BuildingType, BuildingsType>());

            base.Configure(builder);
        }
    }
}
