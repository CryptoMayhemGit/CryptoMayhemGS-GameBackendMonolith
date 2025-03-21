using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class BlockMappingConfiguration : DbTableBaseConfigurationMapping<Block>
    {
        public override void Configure(EntityTypeBuilder<Block> builder)
        {
            builder.ToTable(nameof(Block), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.LastBlock);
            builder.Property(e => e.BlockTypeId);

            builder.HasOne(x => x.BlockType)
                .WithMany(x => x.Blocks)
                .HasForeignKey(x => x.BlockTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
