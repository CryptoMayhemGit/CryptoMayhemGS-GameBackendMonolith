using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class UserLandMappingConfiguration : DbTableBaseConfigurationMapping<UserLand>
    {
        public override void Configure(EntityTypeBuilder<UserLand> builder)
        {
            builder.ToTable(nameof(UserLand), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.LandId);
            builder.Property(e => e.UserId);
            builder.Property(e => e.Owned);
            builder.Property(e => e.HasFog);
            builder.Property(e => e.Status);
            builder.Property(e => e.OnSale);

            builder.HasIndex(e => e.LandId).IsUnique(false);
            builder.HasIndex(e => e.UserId).IsUnique(false);
            builder.HasIndex(e => new { e.LandId, e.UserId }).IsUnique(true);

            builder.HasOne(x => x.Land)
               .WithMany(y => y.UserLands)
               .HasForeignKey(x => x.LandId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
               .WithMany(y => y.UserLands)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
