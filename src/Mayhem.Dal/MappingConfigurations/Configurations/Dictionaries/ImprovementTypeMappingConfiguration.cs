using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Seeds.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries
{
    public class ImprovementTypeMappingConfiguration : DbTableBaseConfigurationMapping<ImprovementType>
    {
        public override void Configure(EntityTypeBuilder<ImprovementType> builder)
        {
            builder.ToTable(nameof(ImprovementType), SchemaNames.DictionarySchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name);

            builder.HasData(EnumToSeedService.GetSeedData<ImprovementType, ImprovementsType>());

            base.Configure(builder);
        }
    }
}
