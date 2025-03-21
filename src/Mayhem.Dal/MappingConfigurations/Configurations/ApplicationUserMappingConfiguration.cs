using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class ApplicationUserMappingConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers", SchemaNames.DefaultSchema);

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UserId);

            builder.HasIndex(e => e.UserId).IsUnique();
        }
    }
}
