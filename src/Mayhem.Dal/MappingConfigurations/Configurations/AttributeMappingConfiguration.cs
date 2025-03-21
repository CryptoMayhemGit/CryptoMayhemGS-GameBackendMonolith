using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class AttributeMappingConfiguration : DbTableBaseConfigurationMapping<Attribute>
    {
        public override void Configure(EntityTypeBuilder<Attribute> builder)
        {
            builder.ToTable(nameof(Attribute), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.NpcId);
            builder.Property(e => e.AttributeTypeId);
            builder.Property(e => e.BaseValue);
            builder.Property(e => e.Value);

            builder.HasOne(x => x.Npc)
               .WithMany(y => y.Attributes)
               .HasForeignKey(x => x.NpcId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AttributeType)
               .WithMany(y => y.Attributes)
               .HasForeignKey(x => x.AttributeTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
