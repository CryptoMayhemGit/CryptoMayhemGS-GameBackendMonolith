using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class UserResourceMappingConfiguration : DbTableBaseConfigurationMapping<UserResource>
    {
        public override void Configure(EntityTypeBuilder<UserResource> builder)
        {
            builder.ToTable(nameof(UserResource), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.UserId);
            builder.Property(e => e.ResourceTypeId);
            builder.Property(e => e.Value);

            builder.HasOne(x => x.ResourceType)
                .WithMany(y => y.UserResources)
                .HasForeignKey(x => x.ResourceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GameUser)
                .WithMany(y => y.UserResources)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
