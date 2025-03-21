using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.DataContext
{
    /// <summary>
    /// // Used in dependecy injection as a context service
    /// </summary>
    public interface IMayhemDataContext : IDisposable
    {
        IDbConnection Connection { get; }
        ChangeTracker ChangeTracker { get; }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Block> Blocks { get; set; }
        DbSet<Tables.Attribute> Attributes { get; set; }
        DbSet<GameUser> GameUsers { get; set; }
        DbSet<Improvement> Improvements { get; set; }
        DbSet<ItemBonus> ItemBonuses { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<LandInstance> LandInstances { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Travel> Travels { get; set; }
        DbSet<UserResource> UserResources { get; set; }
        DbSet<UserLand> UserLands { get; set; }

        DbSet<Building> Buildings { get; set; }
        DbSet<BuildingBonus> BuildingBonuses { get; set; }

        DbSet<AuditLog> AuditLogs { get; set; }

        DbSet<Item> Items { get; set; }
        DbSet<Land> Lands { get; set; }
        DbSet<Npc> Npcs { get; set; }

        DbSet<AttributeType> AttributeTypes { get; set; }
        DbSet<BlockType> BlockTypes { get; set; }
        DbSet<BuildingBonusType> BuildingBonusTypes { get; set; }
        DbSet<BuildingType> BuildingTypes { get; set; }
        DbSet<GuildBuildingBonusType> GuildBuildingBonusTypes { get; set; }
        DbSet<GuildBuildingType> GuildBuildingTypes { get; set; }
        DbSet<GuildImprovementType> GuildImprovementTypes { get; set; }
        DbSet<ImprovementType> ImprovementTypes { get; set; }
        DbSet<ItemBonusType> ItemBonusTypes { get; set; }
        DbSet<ItemType> ItemTypes { get; set; }
        DbSet<NpcType> NpcTypes { get; set; }
        DbSet<ResourceType> ResourceTypes { get; set; }
        DbSet<LandType> LandTypes { get; set; }
        DbSet<NpcStatus> NpcStatuses { get; set; }

        DbSet<Guild> Guilds { get; set; }
        DbSet<GuildBuilding> GuildBuildings { get; set; }
        DbSet<GuildBuildingBonus> GuildBuildingBonuses { get; set; }
        DbSet<GuildImprovement> GuildImprovements { get; set; }
        DbSet<GuildInvitation> GuildInvitations { get; set; }
        DbSet<GuildResource> GuildResources { get; set; }

        DbSet<ExploreMission> ExploreMissions { get; set; }
        DbSet<DiscoveryMission> DiscoveryMissions { get; set; }

        DatabaseFacade Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
