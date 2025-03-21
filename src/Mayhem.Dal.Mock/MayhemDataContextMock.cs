using Mayhem.Dal.Interfaces.Base;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.MappingConfigurations;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Dal.Mock
{
    /// <summary>
    /// Mocked context used in tests - in memory database
    /// </summary>
    public class MayhemDataContextMock : IdentityDbContext<ApplicationUser>, IMayhemDataContext
    {
        private readonly string dbName;

        public MayhemDataContextMock(string dbName)
        {
            this.dbName = dbName;
        }

        public MayhemDataContextMock(DbContextOptions<MayhemDataContextMock> options) : base(options)
        {
        }

        public IDbConnection Connection { get; }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<Tables.Attribute> Attributes { get; set; }
        public virtual DbSet<GameUser> GameUsers { get; set; }
        public virtual DbSet<Improvement> Improvements { get; set; }
        public virtual DbSet<ItemBonus> ItemBonuses { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<LandInstance> LandInstances { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Travel> Travels { get; set; }
        public virtual DbSet<UserResource> UserResources { get; set; }
        public virtual DbSet<UserLand> UserLands { get; set; }

        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<BuildingBonus> BuildingBonuses { get; set; }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Land> Lands { get; set; }
        public virtual DbSet<Npc> Npcs { get; set; }

        public virtual DbSet<AttributeType> AttributeTypes { get; set; }
        public virtual DbSet<BlockType> BlockTypes { get; set; }
        public virtual DbSet<BuildingBonusType> BuildingBonusTypes { get; set; }
        public virtual DbSet<BuildingType> BuildingTypes { get; set; }
        public virtual DbSet<GuildBuildingBonusType> GuildBuildingBonusTypes { get; set; }
        public virtual DbSet<GuildBuildingType> GuildBuildingTypes { get; set; }
        public virtual DbSet<GuildImprovementType> GuildImprovementTypes { get; set; }
        public virtual DbSet<ImprovementType> ImprovementTypes { get; set; }
        public virtual DbSet<ItemBonusType> ItemBonusTypes { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<NpcType> NpcTypes { get; set; }
        public virtual DbSet<ResourceType> ResourceTypes { get; set; }
        public virtual DbSet<LandType> LandTypes { get; set; }
        public virtual DbSet<NpcStatus> NpcStatuses { get; set; }

        public virtual DbSet<Guild> Guilds { get; set; }
        public virtual DbSet<GuildBuilding> GuildBuildings { get; set; }
        public virtual DbSet<GuildBuildingBonus> GuildBuildingBonuses { get; set; }
        public virtual DbSet<GuildImprovement> GuildImprovements { get; set; }
        public virtual DbSet<GuildInvitation> GuildInvitations { get; set; }
        public virtual DbSet<GuildResource> GuildResources { get; set; }

        public virtual DbSet<ExploreMission> ExploreMissions { get; set; }
        public virtual DbSet<DiscoveryMission> DiscoveryMissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(dbName)
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> added = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ITableBase && e.State == EntityState.Added);

            foreach (EntityEntry entityEntry in added)
            {
                ((ITableBase)entityEntry.Entity).CreationDate = DateTime.UtcNow;
            }

            IEnumerable<EntityEntry> modified = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ITableBase && e.State == EntityState.Modified);

            foreach (EntityEntry entityEntry in modified)
            {
                ((ITableBase)entityEntry.Entity).LastModificationDate = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
