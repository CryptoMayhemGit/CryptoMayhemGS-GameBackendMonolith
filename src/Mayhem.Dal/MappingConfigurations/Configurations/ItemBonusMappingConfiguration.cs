using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class ItemBonusMappingConfiguration : DbTableBaseConfigurationMapping<ItemBonus>
    {
        public override void Configure(EntityTypeBuilder<ItemBonus> builder)
        {
            builder.ToTable(nameof(ItemBonus), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.ItemId);
            builder.Property(e => e.ItemBonusTypeId);
            builder.Property(e => e.Bonus);

            builder.HasOne(x => x.ItemBonusType)
                .WithMany(x => x.ItemBonuses)
                .HasForeignKey(x => x.ItemBonusTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany(y => y.ItemBonuses)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
