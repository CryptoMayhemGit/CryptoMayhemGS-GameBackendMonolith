﻿using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Seeds.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Dictionaries
{
    public class GuildImprovementTypeMappingConfiguration : DbTableBaseConfigurationMapping<GuildImprovementType>
    {
        public override void Configure(EntityTypeBuilder<GuildImprovementType> builder)
        {
            builder.ToTable(nameof(GuildImprovementType), SchemaNames.DictionarySchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.Name);

            builder.HasData(EnumToSeedService.GetSeedData<GuildImprovementType, GuildImprovementsType>());

            base.Configure(builder);
        }
    }
}
