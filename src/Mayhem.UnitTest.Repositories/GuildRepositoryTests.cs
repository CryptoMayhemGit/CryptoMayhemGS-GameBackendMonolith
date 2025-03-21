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
    public class GuildRepositoryTests : UnitTestBase
    {
        private IGuildRepository guildRepository;
        private IGuildBuildingRepository guildBuildingRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            guildRepository = GetService<IGuildRepository>();
            guildBuildingRepository = GetService<IGuildBuildingRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task CreateGuild_WhenGuildCreated_ThenGetIt_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            GuildDto guild = await guildRepository.CreateGuildAsync("my guild name", "my guild description", user.Entity.Id);

            guild.Should().NotBeNull();
            guild.GuildResources.Should().HaveCount(Enum.GetValues(typeof(ResourcesType)).Length);
        }

        [Test]
        public async Task CloseGuild_WhenGuildClosed_ThenGetTrue_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> otherUser1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> otherUser2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();


            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "my guild description", user.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, otherUser1.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild.Id, otherUser2.Entity.Id);

            bool result = await guildRepository.CloseGuildAsync(user.Entity.Id);

            Guild removedGuild = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Id);
            List<GuildResource> resources = await mayhemDataContext.GuildResources.Where(x => x.GuildId == guild.Id).ToListAsync();
            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Id).ToListAsync();
            List<GameUser> users = await mayhemDataContext.GameUsers.Where(x => x.GuildId == guild.Id).ToListAsync();
            GameUser owner = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == user.Entity.Id);

            result.Should().BeTrue();
            removedGuild.Should().BeNull();
            user.Entity.GuildId.Should().BeNull();
            resources.Should().HaveCount(0);
            invitations.Should().HaveCount(0);
            users.Should().HaveCount(0);
            owner.GuildId.Should().BeNull();
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenUserInvited_ThenGetInvitation_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);

            GuildInvitation invitationFromDb1 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.Id == invitation.Invitation.Id);
            List<GuildInvitation> invitationFromDb2 = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Id).ToListAsync();
            GuildInvitation invitationFromDb3 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.UserId == user.Entity.Id);

            invitation.Should().NotBeNull();
            invitation.Invitation.Should().NotBeNull();
            invitation.Invitation.Id.Should().BeGreaterThan(0);
            invitationFromDb1.Id.Should().Be(invitation.Invitation.Id);
            invitationFromDb2.Should().HaveCount(1);
            invitationFromDb2.First().Id.Should().Be(invitation.Invitation.Id);
            invitationFromDb3.Id.Should().Be(invitation.Invitation.Id);
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenUserAlreadyAskAboutJoin_ThenAddUserToGuild_Test()
        {
            EntityEntry<GameUser> owner1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild1 = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner1.Entity.Id);
            GuildDto guild2 = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner2.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner2.Entity.Id);

            await guildRepository.AskToJoinGuildByUserAsync(guild1.Id, user.Entity.Id);
            List<GuildInvitation> invitationsBefore = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner1.Entity.Id);

            List<GuildInvitation> invitationsAfter = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            invitation.Should().NotBeNull();
            invitation.AddedUserToGuild.Should().NotBeNull();
            invitationsBefore.Should().HaveCount(2);
            invitationsAfter.Should().BeEmpty();
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserSentInvitation_ThenGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user.Entity.Id);

            GuildInvitation invitationFromDb1 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.Id == invitation.Invitation.Id);
            List<GuildInvitation> invitationFromDb2 = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Id).ToListAsync();
            GuildInvitation invitationFromDb3 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.UserId == user.Entity.Id);

            invitation.Should().NotBeNull();
            invitation.Invitation.Should().NotBeNull();
            invitation.Invitation.Id.Should().BeGreaterThan(0);
            invitationFromDb1.Id.Should().Be(invitation.Invitation.Id);
            invitationFromDb2.Should().HaveCount(1);
            invitationFromDb2.First().Id.Should().Be(invitation.Invitation.Id);
            invitationFromDb3.Id.Should().Be(invitation.Invitation.Id);
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserAlreadyAskAboutJoin_ThenAddUserToGuild_Test()
        {
            EntityEntry<GameUser> owner1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild1 = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner1.Entity.Id);
            GuildDto guild2 = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner2.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner2.Entity.Id);

            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner1.Entity.Id);
            List<GuildInvitation> invitationsBefore = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild1.Id, user.Entity.Id);

            List<GuildInvitation> invitationsAfter = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            invitation.Should().NotBeNull();
            invitation.AddedUserToGuild.Should().NotBeNull();
            invitationsBefore.Should().HaveCount(2);
            invitationsAfter.Should().BeEmpty();
        }

        [Test]
        public async Task AddUserToGuild_WhenUserAdded_ThenGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            AddUserToGuildDto addUserToGuild = await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            addUserToGuild.Should().NotBeNull();
            addUserToGuild.User.Should().NotBeNull();
            addUserToGuild.Guild.Should().NotBeNull();
            addUserToGuild.Guild.Id.Should().Be(guild.Id);
            addUserToGuild.User.Id.Should().Be(user.Entity.Id);
        }

        [Test]
        public async Task AcceptInvitationByOwner_WhenInvitationAccepted_ThenAddUserToGuidAndGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user.Entity.Id);
            AddUserToGuildDto addUserToGuild = await guildRepository.AcceptInvitationByOwnerAsync(invitation.Invitation.Id, owner.Entity.Id);

            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            addUserToGuild.Should().NotBeNull();
            invitations.Should().BeEmpty();
        }

        [Test]
        public async Task DeclineInvitationByOwner_WhenInvitationDeclined_ThenGetSuccess_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user.Entity.Id);
            bool result = await guildRepository.DeclineInvitationByOwnerAsync(invitation.Invitation.Id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationAccepted_ThenAddUserToGuidAndGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);
            AddUserToGuildDto addUserToGuild = await guildRepository.AcceptInvitationByUserAsync(invitation.Invitation.Id, user.Entity.Id);

            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Entity.Id).ToListAsync();

            addUserToGuild.Should().NotBeNull();
            invitations.Should().BeEmpty();
        }

        [Test]
        public async Task DeclineInvitationByUser_WhenInvitationDeclined_ThenGetSuccess_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();
            _ = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            InviteUserDto invitation = await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);
            bool result = await guildRepository.DeclineInvitationByUserAsync(invitation.Invitation.Id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenUserRemoved_ThenGetSuccess_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            bool result = await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserLeft_ThenGetSuccess_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            bool result = await guildRepository.LeaveGuildAsync(user.Entity.Id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerChanged_ThenGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            bool result = await guildRepository.ChangeGuildOwnerAsync(user.Entity.Id, owner.Entity.Id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task GetGuildById_WhenGuildExist_ThenGetIt_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            GuildDto guildDb = await guildRepository.GetGuildByIdAsync(guild.Id);

            guildDb.Should().NotBeNull();
            guildDb.Name.Should().Be(guild.Name);
        }

        [Test]
        public async Task GetGuildsContainsName_WhenGuildsExist_ThenGetThem_Test()
        {
            List<GameUser> owners = new();
            for (int i = 0; i < 50; i++)
            {
                owners.Add((await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity);
            }
            await mayhemDataContext.SaveChangesAsync();

            for (int i = 0; i < 50; i++)
            {
                await guildRepository.CreateGuildAsync($"prefix{i}" + Guid.NewGuid().ToString(), "desc", owners[i].Id);
            }

            IEnumerable<GuildDto> guilds = await guildRepository.GetGuildsAsync(null, null, "prefix1");

            guilds.Should().HaveCount(11);
        }

        [Test]
        public async Task GetGuildsSkipLimit_WhenGuildsExist_ThenGetThem_Test()
        {
            List<GameUser> owners = new();
            for (int i = 0; i < 50; i++)
            {
                owners.Add((await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity);
            }
            await mayhemDataContext.SaveChangesAsync();

            for (int i = 0; i < 50; i++)
            {
                await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owners[i].Id);
            }

            IEnumerable<GuildDto> guilds = await guildRepository.GetGuildsAsync(5, 12, null);

            guilds.Should().HaveCount(12);
        }

        [Test]
        public async Task GetInvitationsByGuildId_WhenInvitationsExist_ThenGetThem_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user3 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user4 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user1.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user2.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user3.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user4.Entity.Id, owner.Entity.Id);

            IEnumerable<GuildInvitationDto> invitations = await guildRepository.GetInvitationsByGuildIdAsync(guild.Id, owner.Entity.Id);

            invitations.Should().HaveCount(3);
        }

        [Test]
        public async Task GetInvitationsByUserId_WhenInvitationsExist_ThenGetThem_Test()
        {
            EntityEntry<GameUser> owner1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner3 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner4 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner1.Entity.Id);
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner2.Entity.Id);
            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner3.Entity.Id);
            GuildDto guild4 = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner4.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner1.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner2.Entity.Id);
            await guildRepository.InviteUserByGuildOwnerAsync(user.Entity.Id, owner3.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild4.Id, user.Entity.Id);

            IEnumerable<GuildInvitationDto> invitations = await guildRepository.GetInvitationsByUserIdAsync(user.Entity.Id);

            invitations.Should().HaveCount(3);
        }

        [Test]
        public async Task AddUserToGuildWithMechBoardBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.MechBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

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
        public async Task AddUserToGuildWithFightBoardBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

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
        public async Task AddUserToGuildWithTransportBoardBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.TransportBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

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
        public async Task AddUserToGuildWithExplorationBoardBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.ExplorationBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

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
        public async Task AddUserToGuildWithAdriaCorporationHeadquartersBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

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
        public async Task RemoveUserFromGuildWithAdriaCorporationHeadquarters_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task RemoveUserFromGuildWithMechBoard_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.MechBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task RemoveUserFromGuildWithExplorationBoard_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.ExplorationBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task RemoveUserFromGuildWithFightBoard_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task RemoveUserFromGuildWithTransportBoard_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.TransportBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.RemoveUserFromGuildByOwnerAsync(user.Entity.Id, owner.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task UserLeaveGuildWithAdriaCorporationHeadquarters_WhenUserRemoved_ThenChangeAttributesForEachNpc_Test()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);
            await guildRepository.LeaveGuildAsync(user.Entity.Id);

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
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task OwnerCloseGuild_WhenGuildClosed_ThenChangeAttributesForEachNpc_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
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
            EntityEntry<GameUser> user2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
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
            EntityEntry<GameUser> user3 = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
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

            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.AdriaCorporationHeadquarters, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.MechBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.FightBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.ExplorationBoard, owner.Entity.Id);
            await guildBuildingRepository.AddGuildBuildingAsync(guild.Id, GuildBuildingsType.TransportBoard, owner.Entity.Id);

            await guildRepository.AddUserToGuildAsync(guild.Id, user1.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user2.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user3.Entity.Id);

            await guildRepository.CloseGuildAsync(owner.Entity.Id);

            GameUser user1Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user1.Entity.Id);

            GameUser user2Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user2.Entity.Id);

            GameUser user3Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user3.Entity.Id);

            foreach (Npc npc in user1Db.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.AdriaCorporationHeadquarters).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }

            foreach (Npc npc in user2Db.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.AdriaCorporationHeadquarters).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }

            foreach (Npc npc in user3Db.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.AdriaCorporationHeadquarters).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().Be(attribute.BaseValue);
                }
            }
        }
    }
}