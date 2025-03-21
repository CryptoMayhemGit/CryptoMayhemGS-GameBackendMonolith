using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class GuildBuildingRepositoryTests : UnitTestBase
    {
        private IGuildBuildingRepository guildBuildingRepository;
        private IMayhemDataContext mayhemDataContext;
        private IGuildRepository guildRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            guildBuildingRepository = GetService<IGuildBuildingRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildRepository = GetService<IGuildRepository>();
        }

        [Test]
        public async Task GetGuildBuildingsByGuildId_WhenBuildingsExists_ThenGetThem_Test()
        {
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild());
            await mayhemDataContext.SaveChangesAsync();

            await mayhemDataContext.GuildBuildings.AddAsync(new GuildBuilding()
            {
                GuildId = guild.Entity.Id,
            });
            await mayhemDataContext.GuildBuildings.AddAsync(new GuildBuilding()
            {
                GuildId = guild.Entity.Id,
            });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<GuildBuildingDto> buildings = await guildBuildingRepository.GetGuildBuildingsByGuildIdAsync(guild.Entity.Id);

            buildings.Should().NotBeNull();
            buildings.Should().HaveCount(2);
        }

        [Test]
        public async Task AddGuildBuilding_WhenBuildingAdded_ThenGetIt_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingDto building = await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, GuildBuildingsType.AdriaCorporationHeadquarters, userId);

            building.Should().NotBeNull();
            building.Level.Should().Be(1);
            building.GuildId.Should().Be(guild.Entity.Id);
            building.GuildBuildingTypeId.Should().Be(GuildBuildingsType.AdriaCorporationHeadquarters);
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenGuildBuildingUpgraded_ThenGetIt_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingDto guildBuilding = await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, GuildBuildingsType.MechBoard, userId);
            GuildBuildingDto guildUpgradedBuilding = await guildBuildingRepository.UpgradeGuildBuildingAsync(guildBuilding.Id, userId);

            guildUpgradedBuilding.Should().NotBeNull();
            guildUpgradedBuilding.Level.Should().Be(guildBuilding.Level + 1);
            guildUpgradedBuilding.GuildBuildingBonuses.Should().HaveCount(1);
            guildUpgradedBuilding.GuildBuildingBonuses.First().Bonus.Should().NotBe(guildBuilding.GuildBuildingBonuses.First().Bonus);
        }

        [Test]
        public async Task UpgradeGuildBuildingManyTimes_WhenGuildBuildingUpgraded_ThenGetIt_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(100_000_000),
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildBuildingDto building = await guildBuildingRepository.AddGuildBuildingAsync(guild.Entity.Id, GuildBuildingsType.TransportBoard, userId);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);
            GuildBuildingDto upgradedBuilding = await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, userId);

            upgradedBuilding.Should().NotBeNull();
            upgradedBuilding.Level.Should().Be(7);
            upgradedBuilding.GuildBuildingBonuses.Should().HaveCount(1);
            Math.Round(upgradedBuilding.GuildBuildingBonuses.First().Bonus, 1).Should().Be(20.4);
        }

        [Test]
        public async Task AddGuildBuildingAdriaCorporationHeadquarters_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.AdriaCorporationHeadquarters).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingMechBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.MechBoard, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.MechBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingFightBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.FightBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingTransportBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.TransportBoard, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.TransportBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingExplorationBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.ExplorationBoard, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.ExplorationBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddAllGuildBuildings_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.MechBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.TransportBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.ExplorationBoard, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.443);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(0.999);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1.53);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.111);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.3);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(0.9);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);

            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.554);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(0.888);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(2.04);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.111);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.4);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(0.8);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);

            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.332);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1.02);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.2);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(1.0);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);
        }

        [Test]
        public async Task UpgradeGuildBuildingFightBoard_WhenBuildingUpgraded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            GuildBuildingDto building = await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.545);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(2.06);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.03);
        }

        [Test]
        public async Task UpgradeGuildBuildingFightBoardManyTime_WhenBuildingUpgraded_ThenChangeAttributesFotEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Scout,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Soldier,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                    },
                    new Npc()
                    {
                        NpcTypeId = NpcsType.Mechanic,
                        Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            GuildBuildingDto building = await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, owner.Entity.Id);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, owner.Entity.Id);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, owner.Entity.Id);
            await guildBuildingRepository.UpgradeGuildBuildingAsync(building.Id, owner.Entity.Id);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.593);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(2.124);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.062);
        }
    }
}
