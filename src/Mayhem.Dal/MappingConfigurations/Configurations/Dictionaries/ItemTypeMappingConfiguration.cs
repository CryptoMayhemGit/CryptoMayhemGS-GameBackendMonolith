using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Seeds.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries
{
    public class ItemTypeMappingConfiguration : DbTableBaseConfigurationMapping<ItemType>
    {
        public override void Configure(EntityTypeBuilder<ItemType> builder)
        {
            builder.ToTable(nameof(ItemType), SchemaNames.DictionarySchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name);

            builder.HasData(EnumToSeedService.GetSeedData<ItemType, ItemsType>());

            base.Configure(builder);
        }
    }
}
