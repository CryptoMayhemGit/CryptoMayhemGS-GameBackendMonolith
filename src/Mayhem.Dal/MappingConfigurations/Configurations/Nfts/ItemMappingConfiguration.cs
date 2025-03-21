using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mayhem.Dal.MappingConfigurations.Configurations.Nfts
{
    public class ItemMappingConfiguration : DbTableBaseConfigurationMapping<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item), SchemaNames.NftSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.UserId);
            builder.Property(e => e.ItemTypeId);
            builder.Property(e => e.IsUsed);
            builder.Property(e => e.Name);
            builder.Property(e => e.Address);
            builder.Property(e => e.IsMinted);

            builder.HasOne(x => x.Npc)
                .WithOne(y => y.Item)
                .HasForeignKey<Npc>(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GameUser)
                .WithMany(y => y.Items)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
