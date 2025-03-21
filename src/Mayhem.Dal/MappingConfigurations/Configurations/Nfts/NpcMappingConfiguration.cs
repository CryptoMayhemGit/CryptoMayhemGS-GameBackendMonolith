using Mayhem.Dal.MappingConfigurations.Configurations.Base;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mayhem.Dal.MappingConfigurations.Configurations.Nfts
{
    public class NpcMappingConfiguration : DbTableBaseConfigurationMapping<Npc>
    {
        public override void Configure(EntityTypeBuilder<Npc> builder)
        {
            builder.ToTable(nameof(Npc), SchemaNames.NftSchema);

            builder.Property(e => e.Id);
            builder.Property(e => e.UserId);
            builder.Property(e => e.NpcTypeId);
            builder.Property(e => e.NpcHealthStateId);
            builder.Property(e => e.ItemId);
            builder.Property(e => e.BuildingId);
            builder.Property(e => e.Name);
            builder.Property(e => e.Address);
            builder.Property(e => e.IsAvatar);
            builder.Property(e => e.IsMinted);
            builder.Property(e => e.NpcStatusId);
            builder.Property(e => e.LandId);

            builder.HasOne(x => x.NpcType)
                .WithMany(x => x.Npcs)
                .HasForeignKey(x => x.NpcTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Land)
                .WithMany(x => x.Npcs)
                .HasForeignKey(x => x.LandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GameUser)
                .WithMany(y => y.Npcs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Building)
                .WithMany(x => x.Npcs)
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.NpcStatus)
                .WithMany(x => x.Npcs)
                .HasForeignKey(x => x.NpcStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
