using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations.Nfts
{
    public class LandMappingConfiguration : DbTableBaseConfigurationMapping<Land>
    {
        public override void Configure(EntityTypeBuilder<Land> builder)
        {
            builder.ToTable(nameof(Land), SchemaNames.NftSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.LandInstanceId).IsRequired();
            builder.Property(e => e.PositionX).IsRequired();
            builder.Property(e => e.PositionY).IsRequired();
            builder.Property(e => e.Name);
            builder.Property(e => e.IsMinted);

            builder.HasIndex(x => x.LandInstanceId);
            builder.HasIndex(x => new { x.PositionX, x.PositionY });
            builder.HasIndex(x => new { x.LandInstanceId, x.PositionX, x.PositionY }).IsUnique();

            builder.HasOne(x => x.LandInstance)
                .WithMany(x => x.Lands)
                .HasForeignKey(x => x.LandInstanceId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
