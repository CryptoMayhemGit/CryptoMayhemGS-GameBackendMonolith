using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingConfigurations.Configurations
{
    public class TravelMappingConfiguration : DbTableBaseConfigurationMapping<Travel>
    {
        public override void Configure(EntityTypeBuilder<Travel> builder)
        {
            builder.ToTable(nameof(Travel), SchemaNames.DefaultSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.NpcId);
            builder.Property(e => e.LandFromId);
            builder.Property(e => e.LandToId);
            builder.Property(e => e.LandTargetId); 
            builder.Property(e => e.FinishDate);

            builder.HasIndex(e => new { e.NpcId, e.LandFromId, e.LandToId }).IsUnique();

            builder.HasOne(x => x.Npc)
               .WithMany(y => y.Travels)
               .HasForeignKey(x => x.NpcId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LandFrom)
               .WithMany(y => y.TravelsFrom)
               .HasForeignKey(x => x.LandFromId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LandTo)
               .WithMany(y => y.TravelsTo)
               .HasForeignKey(x => x.LandToId)
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
