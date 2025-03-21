using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Test.Common;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.AcceptInvitationByOwner;
using Mayhen.Bl.Commands.AcceptInvitationByUser;
using Mayhen.Bl.Commands.AddGuildBuilding;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Commands.ChangeGuildOwner;
using Mayhen.Bl.Commands.CloseGuild;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Commands.DeclineInvitationByOwner;
using Mayhen.Bl.Commands.DeclineInvitationByUser;
using Mayhen.Bl.Commands.GetGuildById;
using Mayhen.Bl.Commands.GetGuilds;
using Mayhen.Bl.Commands.GetInvitationsByGuildId;
using Mayhen.Bl.Commands.GetInvitationsByUserId;
using Mayhen.Bl.Commands.InviteUserByGuildOwner;
using Mayhen.Bl.Commands.LeaveGuild;
using Mayhen.Bl.Commands.RemoveUserFromGuildByOwner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class GuildControllerTests : GuildTestBase<GuildControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildRepository guildRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildRepository = GetService<IGuildRepository>();
        }

        [Test]
        public async Task CreateGuild_WhenGuildCreated_ThenGetIt_Test()
        {
            string name = Guid.NewGuid().ToString();
            ActionDataResult<CreateGuildCommandResponse> response = await CreateGuildAsync(name, Token);

            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == response.Result.Guild.Id);

            response.Should().NotBeNull();
            response.Result.Guild.GuildResources.Should().HaveCount(Enum.GetValues(typeof(ResourcesType)).Length);
            guildDb.Name.Should().Be(response.Result.Guild.Name);
        }

        [Test]
        public async Task CreateGuild_WhenGuildWithThisNameExist_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();
            string guildName = Guid.NewGuid().ToString();
            await CreateGuildAsync(guildName, token);
            ActionDataResult<CreateGuildCommandResponse> response = await CreateGuildAsync(guildName, token);

            response.Errors.First().Message.Should().Be($"Guild with name {guildName} already exists.");
            response.Errors.First().FieldName.Should().Be($"GuildName");
        }

        [Test]
        public async Task CreateGuild_WhenOwnerNotExist_ThenGetErrors_Test()
        {
            (int id, string token) = GetFakeToken();
            ActionDataResult<CreateGuildCommandResponse> response = await CreateGuildAsync(Guid.NewGuid().ToString(), token);

            response.Errors.First().Message.Should().Be($"User with id {id} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task CreateGuild_WhenOwnerHasOtherGuild_ThenGetErrors_Test()
        {
            (int id, string token) = await GetNewTokenAsync();
            await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            ActionDataResult<CreateGuildCommandResponse> response = await CreateGuildAsync(Guid.NewGuid().ToString(), token);

            response.Errors.First().Message.Should().Be($"User with id {id} already has guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task CloseGuild_WhenGuildClosed_ThenGetTrue_Test()
        {
            (int id, string token) = await GetNewTokenAsync();
            EntityEntry<GameUser> otherUser1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> otherUser2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, otherUser1.Entity.Id);
            await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, token);

            ActionDataResult<CloseGuildCommandResponse> response = await CloseGuildAsync(token);

            Guild removedGuild = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            List<GuildResource> resources = await mayhemDataContext.GuildResources.Where(x => x.GuildId == guild.Result.Guild.Id).ToListAsync();
            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Result.Guild.Id).ToListAsync();
            List<GameUser> users = await mayhemDataContext.GameUsers.Where(x => x.GuildId == guild.Result.Guild.Id).ToListAsync();
            GameUser owner = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == id);

            response.Result.Result.Should().BeTrue();
            removedGuild.Should().BeNull();
            resources.Should().HaveCount(0);
            invitations.Should().HaveCount(0);
            users.Should().HaveCount(0);
            owner.GuildId.Should().BeNull();
        }

        [Test]
        public async Task CloseGuild_WhenOwnerNotExist_ThenGetErrors_Test()
        {
            (_, string token) = GetFakeToken();
            ActionDataResult<CloseGuildCommandResponse> response = await CloseGuildAsync(token);

            response.Errors.First().Message.Should().Be($"User isn't guild owner.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenUserInvited_ThenGetInvitation_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(user.Entity.Id, token);

            GuildInvitation invitationFromDb1 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.Id == invitation.Result.InviteUser.Invitation.Id);
            List<GuildInvitation> invitationFromDb2 = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Result.Guild.Id).ToListAsync();
            GuildInvitation invitationFromDb3 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.UserId == user.Entity.Id);

            invitation.Should().NotBeNull();
            invitation.Result.InviteUser.Invitation.Should().NotBeNull();
            invitation.Result.InviteUser.Invitation.Id.Should().BeGreaterThan(0);
            invitationFromDb1.Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
            invitationFromDb2.Should().HaveCount(1);
            invitationFromDb2.First().Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
            invitationFromDb3.Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserIsOwner_ThenGetErrors_Test()
        {
            (int id, string token) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> response = await InviteUserByGuildOwnerAsync(id, token);

            response.Errors.First().Message.Should().Be($"UserId: User cannot invite himself.");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenOwnerNotOwnGuild_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> response = await InviteUserByGuildOwnerAsync(user.Entity.Id, token);

            response.Errors.First().Message.Should().Be($"User doesn't have guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserNotExist_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> response = await InviteUserByGuildOwnerAsync(123123, token);

            response.Errors.First().Message.Should().Be($"Invited user doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"InvitedUserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserHasOtherGuild_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, user.Entity.Id);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> response = await InviteUserByGuildOwnerAsync(user.Entity.Id, token);

            response.Errors.First().Message.Should().Be($"Invited user is already in guild.");
            response.Errors.First().FieldName.Should().Be($"InvitedUserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenInvitedUserAlreadyReceivedInvitation_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            await InviteUserByGuildOwnerAsync(user.Entity.Id, token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> response = await InviteUserByGuildOwnerAsync(user.Entity.Id, token);

            response.Errors.First().Message.Should().Be($"User has already been invited.");
            response.Errors.First().FieldName.Should().Be($"InvitedUserId");
        }

        [Test]
        public async Task InviteUserByGuildOwner_WhenUserAlreadyAskAboutJoin_ThenAddUserToGuild_Test()
        {
            (int userId, string userToken) = await GetNewTokenAsync();
            (_, string token1) = await GetNewTokenAsync();
            (_, string token2) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild1 = await CreateGuildAsync(Guid.NewGuid().ToString(), token1);
            ActionDataResult<CreateGuildCommandResponse> guild2 = await CreateGuildAsync(Guid.NewGuid().ToString(), token2);
            await InviteUserByGuildOwnerAsync(userId, token2);

            await AskToJoinGuildByUserAsync(guild1.Result.Guild.Id, userToken);
            List<GuildInvitation> invitationsBefore = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(userId, token1);

            List<GuildInvitation> invitationsAfter = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            invitation.Should().NotBeNull();
            invitation.Result.InviteUser.AddedUserToGuild.Should().NotBeNull();
            invitationsBefore.Should().HaveCount(2);
            invitationsAfter.Should().BeEmpty();
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserSentInvitation_ThenGetIt_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);

            GuildInvitation invitationFromDb1 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.Id == invitation.Result.InviteUser.Invitation.Id);
            List<GuildInvitation> invitationFromDb2 = await mayhemDataContext.GuildInvitations.Where(x => x.GuildId == guild.Result.Guild.Id).ToListAsync();
            GuildInvitation invitationFromDb3 = await mayhemDataContext.GuildInvitations.SingleOrDefaultAsync(x => x.UserId == userId);

            invitation.Should().NotBeNull();
            invitation.Result.InviteUser.Invitation.Should().NotBeNull();
            invitation.Result.InviteUser.Invitation.Id.Should().BeGreaterThan(0);
            invitationFromDb1.Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
            invitationFromDb2.Should().HaveCount(1);
            invitationFromDb2.First().Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
            invitationFromDb3.Id.Should().Be(invitation.Result.InviteUser.Invitation.Id);
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserNotExist_ThenGetErrors_Test()
        {
            (int fakeId, string fakeToken) = GetFakeToken();
            (_, string token) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> response = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, fakeToken);

            response.Errors.First().Message.Should().Be($"User with id {fakeId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserIsInOtherGuild_ThenGetErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, userId);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> response = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);

            response.Errors.First().Message.Should().Be($"User with id {userId} is already in other guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenGuildNotExist_ThenGetErrors_Test()
        {
            (_, string userToken) = await GetNewTokenAsync();

            int guidId = 23141332;

            ActionDataResult<AskToJoinGuildByUserCommandResponse> response = await AskToJoinGuildByUserAsync(guidId, userToken);

            response.Errors.First().Message.Should().Be($"Guild with id {guidId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"GuildId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenGuildAlreadyReceivedInquiry_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (_, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> response = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);

            response.Errors.First().Message.Should().Be($"User has already sent an invitation.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserAlreadyAskAboutJoin_ThenAddUserToGuild_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild1 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);
            ActionDataResult<CreateGuildCommandResponse> guild2 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner2Token);
            await InviteUserByGuildOwnerAsync(userId, owner2Token);

            await InviteUserByGuildOwnerAsync(userId, owner1Token);
            List<GuildInvitation> invitationsBefore = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            InviteUserDto invitation = await guildRepository.AskToJoinGuildByUserAsync(guild1.Result.Guild.Id, userId);

            List<GuildInvitation> invitationsAfter = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            invitation.Should().NotBeNull();
            invitation.AddedUserToGuild.Should().NotBeNull();
            invitationsBefore.Should().HaveCount(2);
            invitationsAfter.Should().BeEmpty();
        }

        [Test]
        public async Task AcceptInvitationByOwner_WhenInvitationAccepted_ThenAddUserToGuidAndGetIt_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            ActionDataResult<AcceptInvitationByOwnerCommandResponse> addUserToGuild = await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            addUserToGuild.Should().NotBeNull();
            invitations.Should().BeEmpty();
        }

        [Test]
        public async Task AcceptInvitationByOwner_WhenInvitationNotExist_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();

            int invitationId = 50375;
            ActionDataResult<AcceptInvitationByOwnerCommandResponse> response = await AcceptInvitationByOwnerAsync(invitationId, ownerToken);

            response.Errors.First().Message.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByOwner_WhenInvitationIsAcceptedByNotOwner_ThenGetErrors_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (_, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            ActionDataResult<AcceptInvitationByOwnerCommandResponse> response = await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, owner2Token);

            response.Errors.First().Message.Should().Be($"Only guild owner can accept the invitation.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task DeclineInvitationByOwner_WhenInvitationDeclined_ThenGetSuccess_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (_, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            ActionDataResult<DeclineInvitationByOwnerCommandResponse> response = await DeclineInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task DeclineInvitationByOwner_WhenInvitationNotExist_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();

            int invitationId = 9381;
            ActionDataResult<DeclineInvitationByOwnerCommandResponse> response = await DeclineInvitationByOwnerAsync(invitationId, ownerToken);

            response.Errors.First().Message.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task DeclineInvitationByOwner_WhenInvitationIsDeclinedByNotOwner_ThenGetErrors_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (_, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);

            ActionDataResult<DeclineInvitationByOwnerCommandResponse> response = await DeclineInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, owner2Token);

            response.Errors.First().Message.Should().Be($"Only guild owner can decline the invitation.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationAccepted_ThenAddUserToGuidAndGetIt_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(userId, ownerToken);
            ActionDataResult<AcceptInvitationByUserCommandResponse> addUserToGuild = await AcceptInvitationByUserAsync(invitation.Result.InviteUser.Invitation.Id, userToken);

            List<GuildInvitation> invitations = await mayhemDataContext.GuildInvitations.Where(x => x.UserId == userId).ToListAsync();

            addUserToGuild.Should().NotBeNull();
            invitations.Should().BeEmpty();
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationNotExist_ThenGetErrors_Test()
        {
            (_, string userToken) = await GetNewTokenAsync();

            int invitationId = 50375;
            ActionDataResult<AcceptInvitationByUserCommandResponse> response = await AcceptInvitationByUserAsync(invitationId, userToken);

            response.Errors.First().Message.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task AcceptInvitationByUser_WhenInvitationIsAcceptedByNotInvitedUser_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int user1Id, string _) = await GetNewTokenAsync();
            (int _, string user2Token) = await GetNewTokenAsync();
            _ = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(user1Id, ownerToken);
            ActionDataResult<AcceptInvitationByUserCommandResponse> response = await AcceptInvitationByUserAsync(invitation.Result.InviteUser.Invitation.Id, user2Token);

            response.Errors.First().Message.Should().Be($"Only user from invitation can accept.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task DeclineInvitationByUser_WhenInvitationDeclined_ThenGetSuccess_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();
            _ = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(userId, ownerToken);
            ActionDataResult<DeclineInvitationByUserCommandResponse> response = await DeclineInvitationByUserAsync(invitation.Result.InviteUser.Invitation.Id, userToken);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task DeclineInvitationByUser_WhenInvitationNotExist_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();

            int invitationId = 9381;
            ActionDataResult<DeclineInvitationByUserCommandResponse> response = await DeclineInvitationByUserAsync(invitationId, ownerToken);

            response.Errors.First().Message.Should().Be($"Invitation with id {invitationId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task DeclineInvitationByUser_WhenInvitationIsDeclinedByNotOwner_ThenGetErrors_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (int userId, string _) = await GetNewTokenAsync();
            _ = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(userId, owner1Token);

            ActionDataResult<DeclineInvitationByUserCommandResponse> response = await DeclineInvitationByUserAsync(invitation.Result.InviteUser.Invitation.Id, owner2Token);

            response.Errors.First().Message.Should().Be($"Only user from invitation can decline.");
            response.Errors.First().FieldName.Should().Be($"InvitationId");
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenUserRemoved_ThenGetSuccess_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, _) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, userId);
            ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse> response = await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenUserIsNotOwner_ThenGetErrors_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (int userId, _) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, userId);
            ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse> response = await RemoveUserFromGuildByOwnerAsync(userId, owner2Token);

            response.Errors.First().Message.Should().Be($"User doesn't have guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenUserNotBelongsToGuild_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, _) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse> response = await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            response.Errors.First().Message.Should().Be($"User with id {userId} doesn't belong to the guild {guild.Result.Guild.Name}.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task RemoveUserFromGuildByOwner_WhenOwnerRemoveHimself_ThenGetErrors_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse> response = await RemoveUserFromGuildByOwnerAsync(ownerId, ownerToken);

            response.Errors.First().Message.Should().Be($"Owner cannot remove himself.");
            response.Errors.First().FieldName.Should().Be($"RemovedUserId");
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserLeft_ThenGetSuccess_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, userId);
            ActionDataResult<LeaveGuildCommandResponse> response = await LeaveGuildAsync(userToken);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserNotExist_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = GetFakeToken();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<LeaveGuildCommandResponse> response = await LeaveGuildAsync(userToken);

            response.Errors.First().Message.Should().Be($"User with id {userId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserInNotInGuild_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (_, string userToken) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<LeaveGuildCommandResponse> response = await LeaveGuildAsync(userToken);

            response.Errors.First().Message.Should().Be($"User doesn't have guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task LeaveGuildAsync_WhenUserIsOwner_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<LeaveGuildCommandResponse> response = await LeaveGuildAsync(ownerToken);

            response.Errors.First().Message.Should().Be($"User cannot leave the guild he owns.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerChanged_ThenGetIt_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, _) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await guildRepository.AddUserToGuildAsync(guild.Result.Guild.Id, userId);
            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(userId, ownerToken);

            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerNotExist_ThenGetErrors_Test()
        {
            (int ownerId, string ownerToken) = GetFakeToken();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(21387128, ownerToken);

            response.Errors.First().Message.Should().Be($"User with id {ownerId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenOwnerNotHaveGuild_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, _) = GetFakeToken();

            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(userId, ownerToken);

            response.Errors.First().Message.Should().Be($"User doesn't have guild.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenUserIsNotOwner_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int user1Id, _) = await GetNewTokenAsync();
            (int user2Id, string user2Token) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild1 = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await guildRepository.AddUserToGuildAsync(guild1.Result.Guild.Id, user1Id);
            await guildRepository.AddUserToGuildAsync(guild1.Result.Guild.Id, user2Id);
            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(user1Id, user2Token);

            response.Errors.First().Message.Should().Be($"User isn't guild owner.");
            response.Errors.First().FieldName.Should().Be($"UserId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenNewOwnerNotBelongsToGuild_ThenGetErrors_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, _) = GetFakeToken();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(userId, ownerToken);

            response.Errors.First().Message.Should().Be($"New owner doesn't belong to the guild.");
            response.Errors.First().FieldName.Should().Be($"NewOwnerId");
        }

        [Test]
        public async Task ChangeGuildOwner_WhenNewOwnerIsOldOwner_ThenGetErrors_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            ActionDataResult<ChangeGuildOwnerCommandResponse> response = await ChangeGuildOwnerAsync(ownerId, ownerToken);

            response.Errors.First().Message.Should().Be($"Owner cannot change owner to himself.");
            response.Errors.First().FieldName.Should().Be($"NewOwnerId");
        }

        [Test]
        public async Task GetGuildById_WhenGuildExist_ThenGetIt_Test()
        {
            (_, string token) = await GetNewTokenAsync();
            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), token);

            ActionDataResult<GetGuildByIdCommandResponse> response = await GetGuildByIdAsync(guild.Result.Guild.Id, token);

            response.Result.Guild.Should().NotBeNull();
            response.Result.Guild.Name.Should().Be(guild.Result.Guild.Name);
        }

        [Test]
        public async Task GetGuildsContainsName_WhenGuildsExist_ThenGetThem_Test()
        {
            List<(int userId, string token)> owners = new();
            for (int i = 0; i < 50; i++)
            {
                owners.Add(await GetNewTokenAsync());
            }

            for (int i = 0; i < 50; i++)
            {
                await CreateGuildAsync($"prefix{i}" + Guid.NewGuid().ToString(), owners[i].token);
            }

            ActionDataResult<GetGuildsCommandResponse> response = await GetGuildsAsync(null, null, "prefix1", Token);

            response.Result.Guilds.Should().HaveCount(11);
        }

        [Test]
        public async Task GetGuildsSkipLimit_WhenGuildsExist_ThenGetThem_Test()
        {
            List<(int userId, string token)> owners = new();
            for (int i = 0; i < 50; i++)
            {
                owners.Add(await GetNewTokenAsync());
            }

            for (int i = 0; i < 50; i++)
            {
                await CreateGuildAsync(Guid.NewGuid().ToString(), owners[i].token);
            }

            ActionDataResult<GetGuildsCommandResponse> response = await GetGuildsAsync(5, 12, null, Token);

            response.Result.Guilds.Should().HaveCount(12);
        }

        [Test]
        public async Task GetInvitationsByGuildId_WhenInvitationsExist_ThenGetThem_Test()
        {
            (_, string ownerToken) = await GetNewTokenAsync();
            (_, string user1Token) = await GetNewTokenAsync();
            (_, string user2Token) = await GetNewTokenAsync();
            (_, string user3Token) = await GetNewTokenAsync();
            (int user4Id, _) = await GetNewTokenAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user1Token);
            await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user2Token);
            await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user3Token);
            await InviteUserByGuildOwnerAsync(user4Id, ownerToken);

            ActionDataResult<GetInvitationsByGuildIdCommandResponse> response = await GetInvitationsByGuildIdAsync(guild.Result.Guild.Id, ownerToken);

            response.Result.Invitations.Should().HaveCount(3);
        }

        [Test]
        public async Task GetInvitationsByUserId_WhenInvitationsExist_ThenGetThem_Test()
        {
            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (_, string owner3Token) = await GetNewTokenAsync();
            (_, string owner4Token) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);
            await CreateGuildAsync(Guid.NewGuid().ToString(), owner2Token);
            await CreateGuildAsync(Guid.NewGuid().ToString(), owner3Token);
            ActionDataResult<CreateGuildCommandResponse> guild4 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner4Token);
            await InviteUserByGuildOwnerAsync(userId, owner1Token);
            await InviteUserByGuildOwnerAsync(userId, owner2Token);
            await InviteUserByGuildOwnerAsync(userId, owner3Token);
            await AskToJoinGuildByUserAsync(guild4.Result.Guild.Id, userToken);

            ActionDataResult<GetInvitationsByUserIdCommandResponse> response = await GetInvitationsByUserIdAsync(userToken);

            response.Result.Invitations.Should().HaveCount(3);
        }

        [Test]
        public async Task Invitations_E2E_Test()
        {
            // Scenario:

            // 1. User create own guild ->
            // 2. invites 2 users ->
            // 3. first user accept invite ->
            // 4. second user decline invite ->
            // 5. meanwhile other two users ask to join ->
            // 6. owner accept first ->
            // 7. refuse the other ->
            // 8. another user asks to join ->
            // 9. owner also sends him invitation

            // Expected result:
            // Three people in guild

            (_, string ownerToken) = await GetNewTokenAsync();
            (int user1Id, string user1Token) = await GetNewTokenAsync();
            (int user2Id, string user2Token) = await GetNewTokenAsync();
            (_, string user3Token) = await GetNewTokenAsync();
            (_, string user4Token) = await GetNewTokenAsync();
            (int user5Id, string user5Token) = await GetNewTokenAsync();

            // 1.
            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            // 2.
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation1 = await InviteUserByGuildOwnerAsync(user1Id, ownerToken);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation2 = await InviteUserByGuildOwnerAsync(user2Id, ownerToken);

            // 3.
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser = await AcceptInvitationByUserAsync(invitation1.Result.InviteUser.Invitation.Id, user1Token);

            // 4.
            ActionDataResult<DeclineInvitationByUserCommandResponse> declinedByUser = await DeclineInvitationByUserAsync(invitation2.Result.InviteUser.Invitation.Id, user2Token);

            // 5.
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation3 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user3Token);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation4 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user4Token);

            // 6.
            ActionDataResult<AcceptInvitationByOwnerCommandResponse> acceptedByOwner = await AcceptInvitationByOwnerAsync(invitation3.Result.InviteUser.Invitation.Id, ownerToken);

            // 7.
            ActionDataResult<DeclineInvitationByOwnerCommandResponse> declinedByOwner = await DeclineInvitationByOwnerAsync(invitation4.Result.InviteUser.Invitation.Id, ownerToken);

            // 8.
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation5 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user5Token);

            // 9.
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation6 = await InviteUserByGuildOwnerAsync(user5Id, ownerToken);

            ActionDataResult<GetGuildByIdCommandResponse> guildById = await GetGuildByIdAsync(guild.Result.Guild.Id, ownerToken);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations = await GetInvitationsByGuildIdAsync(guild.Result.Guild.Id, ownerToken);

            guild.IsSuccessStatusCode.Should().BeTrue();
            invitation1.IsSuccessStatusCode.Should().BeTrue();
            invitation2.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser.IsSuccessStatusCode.Should().BeTrue();
            declinedByUser.IsSuccessStatusCode.Should().BeTrue();
            invitation3.IsSuccessStatusCode.Should().BeTrue();
            invitation4.IsSuccessStatusCode.Should().BeTrue();
            acceptedByOwner.IsSuccessStatusCode.Should().BeTrue();
            declinedByOwner.IsSuccessStatusCode.Should().BeTrue();
            invitation5.IsSuccessStatusCode.Should().BeTrue();
            invitation6.IsSuccessStatusCode.Should().BeTrue();
            guildById.IsSuccessStatusCode.Should().BeTrue();
            invitations.IsSuccessStatusCode.Should().BeTrue();

            guildById.Result.Guild.Users.Should().HaveCount(4);
            invitations.Result.Invitations.Should().HaveCount(0);
        }

        [Test]
        public async Task ChangeOwner_E2E_Test()
        {
            // Scenario:

            // 1. User create own guild ->
            // 2. invites 1 user ->
            // 3. user accept invitation ->
            // 4. owner change guild owner to new user

            // Expected result:
            // Guild has new owner

            (_, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            // 1.
            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            // 2.
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation = await InviteUserByGuildOwnerAsync(userId, ownerToken);

            // 3.
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser = await AcceptInvitationByUserAsync(invitation.Result.InviteUser.Invitation.Id, userToken);

            // 4.
            ActionDataResult<ChangeGuildOwnerCommandResponse> changeGuildOwnerResponse = await ChangeGuildOwnerAsync(userId, ownerToken);

            ActionDataResult<GetGuildByIdCommandResponse> guildById = await GetGuildByIdAsync(guild.Result.Guild.Id, ownerToken);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations = await GetInvitationsByGuildIdAsync(guild.Result.Guild.Id, ownerToken);

            guild.IsSuccessStatusCode.Should().BeTrue();
            invitation.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser.IsSuccessStatusCode.Should().BeTrue();
            changeGuildOwnerResponse.IsSuccessStatusCode.Should().BeTrue();
            invitations.IsSuccessStatusCode.Should().BeTrue();
            guildById.IsSuccessStatusCode.Should().BeTrue();

            guildById.Result.Guild.OwnerId.Should().Be(userId);
            invitations.Result.Invitations.Should().HaveCount(0);
        }

        [Test]
        public async Task AddRemoveUser_E2E_Test()
        {
            // Scenario:

            // 1. User create own guild ->
            // 2. invites 4 user ->
            // 3. users accept invitation ->
            // 4. two user leave guild ->
            // 5. one users are removed by owner

            // Expected result:
            // Guild has two users

            (_, string ownerToken) = await GetNewTokenAsync();
            (int user1Id, string user1Token) = await GetNewTokenAsync();
            (int user2Id, string user2Token) = await GetNewTokenAsync();
            (int user3Id, string user3Token) = await GetNewTokenAsync();
            (int user4Id, string user4Token) = await GetNewTokenAsync();

            // 1.
            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);

            // 2.
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation1 = await InviteUserByGuildOwnerAsync(user1Id, ownerToken);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation2 = await InviteUserByGuildOwnerAsync(user2Id, ownerToken);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation3 = await InviteUserByGuildOwnerAsync(user3Id, ownerToken);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation4 = await InviteUserByGuildOwnerAsync(user4Id, ownerToken);

            // 3.
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser1 = await AcceptInvitationByUserAsync(invitation1.Result.InviteUser.Invitation.Id, user1Token);
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser2 = await AcceptInvitationByUserAsync(invitation2.Result.InviteUser.Invitation.Id, user2Token);
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser3 = await AcceptInvitationByUserAsync(invitation3.Result.InviteUser.Invitation.Id, user3Token);
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser4 = await AcceptInvitationByUserAsync(invitation4.Result.InviteUser.Invitation.Id, user4Token);

            // 4.
            ActionDataResult<LeaveGuildCommandResponse> leaveGuild1 = await LeaveGuildAsync(user1Token);
            ActionDataResult<LeaveGuildCommandResponse> leaveGuild2 = await LeaveGuildAsync(user2Token);

            // 5.
            ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse> removeUserFromGuild = await RemoveUserFromGuildByOwnerAsync(user3Id, ownerToken);

            ActionDataResult<GetGuildByIdCommandResponse> guildById = await GetGuildByIdAsync(guild.Result.Guild.Id, ownerToken);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations = await GetInvitationsByGuildIdAsync(guild.Result.Guild.Id, ownerToken);

            guild.IsSuccessStatusCode.Should().BeTrue();
            invitation1.IsSuccessStatusCode.Should().BeTrue();
            invitation2.IsSuccessStatusCode.Should().BeTrue();
            invitation3.IsSuccessStatusCode.Should().BeTrue();
            invitation4.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser1.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser2.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser3.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser4.IsSuccessStatusCode.Should().BeTrue();
            leaveGuild1.IsSuccessStatusCode.Should().BeTrue();
            leaveGuild2.IsSuccessStatusCode.Should().BeTrue();
            removeUserFromGuild.IsSuccessStatusCode.Should().BeTrue();
            guildById.IsSuccessStatusCode.Should().BeTrue();
            invitations.IsSuccessStatusCode.Should().BeTrue();

            guildById.Result.Guild.Users.Should().HaveCount(2);
            invitations.Result.Invitations.Should().HaveCount(0);
        }

        [Test]
        public async Task FewGuilds_E2E_Test()
        {
            // Scenario:

            // 1. Three owners create own guild ->
            // 2. all invite users
            // 3. first user join to guild 1 ->
            // 4. second user join to guild 2

            // Expected result:
            // Guild 1 has two user, guild 2 has 2 user, guild 3 has only owner

            (_, string owner1Token) = await GetNewTokenAsync();
            (_, string owner2Token) = await GetNewTokenAsync();
            (_, string owner3Token) = await GetNewTokenAsync();
            (int user1Id, string user1Token) = await GetNewTokenAsync();
            (int user2Id, string user2Token) = await GetNewTokenAsync();

            // 1.
            ActionDataResult<CreateGuildCommandResponse> guild1 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner1Token);
            ActionDataResult<CreateGuildCommandResponse> guild2 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner2Token);
            ActionDataResult<CreateGuildCommandResponse> guild3 = await CreateGuildAsync(Guid.NewGuid().ToString(), owner3Token);

            // 2.
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation1 = await InviteUserByGuildOwnerAsync(user1Id, owner1Token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation2 = await InviteUserByGuildOwnerAsync(user1Id, owner2Token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation3 = await InviteUserByGuildOwnerAsync(user1Id, owner3Token);

            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation4 = await InviteUserByGuildOwnerAsync(user2Id, owner1Token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation5 = await InviteUserByGuildOwnerAsync(user2Id, owner2Token);
            ActionDataResult<InviteUserByGuildOwnerCommandResponse> invitation6 = await InviteUserByGuildOwnerAsync(user2Id, owner3Token);

            // 3.
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser1 = await AcceptInvitationByUserAsync(invitation1.Result.InviteUser.Invitation.Id, user1Token);

            // 4.
            ActionDataResult<AcceptInvitationByUserCommandResponse> acceptedByUser2 = await AcceptInvitationByUserAsync(invitation5.Result.InviteUser.Invitation.Id, user2Token);

            ActionDataResult<GetGuildByIdCommandResponse> guild1ById = await GetGuildByIdAsync(guild1.Result.Guild.Id, owner1Token);
            ActionDataResult<GetGuildByIdCommandResponse> guild2ById = await GetGuildByIdAsync(guild2.Result.Guild.Id, owner2Token);
            ActionDataResult<GetGuildByIdCommandResponse> guild3ById = await GetGuildByIdAsync(guild3.Result.Guild.Id, owner3Token);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations1 = await GetInvitationsByGuildIdAsync(guild1.Result.Guild.Id, owner1Token);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations2 = await GetInvitationsByGuildIdAsync(guild2.Result.Guild.Id, owner2Token);
            ActionDataResult<GetInvitationsByGuildIdCommandResponse> invitations3 = await GetInvitationsByGuildIdAsync(guild3.Result.Guild.Id, owner3Token);

            guild1.IsSuccessStatusCode.Should().BeTrue();
            guild2.IsSuccessStatusCode.Should().BeTrue();
            guild3.IsSuccessStatusCode.Should().BeTrue();
            invitation1.IsSuccessStatusCode.Should().BeTrue();
            invitation2.IsSuccessStatusCode.Should().BeTrue();
            invitation3.IsSuccessStatusCode.Should().BeTrue();
            invitation4.IsSuccessStatusCode.Should().BeTrue();
            invitation5.IsSuccessStatusCode.Should().BeTrue();
            invitation6.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser1.IsSuccessStatusCode.Should().BeTrue();
            acceptedByUser2.IsSuccessStatusCode.Should().BeTrue();
            guild1ById.IsSuccessStatusCode.Should().BeTrue();
            guild2ById.IsSuccessStatusCode.Should().BeTrue();
            guild3ById.IsSuccessStatusCode.Should().BeTrue();
            invitations1.IsSuccessStatusCode.Should().BeTrue();
            invitations2.IsSuccessStatusCode.Should().BeTrue();
            invitations3.IsSuccessStatusCode.Should().BeTrue();

            invitations1.Result.Invitations.Should().HaveCount(0);
            invitations2.Result.Invitations.Should().HaveCount(0);
            invitations3.Result.Invitations.Should().HaveCount(0);
            guild1ById.Result.Guild.Users.Should().HaveCount(2);
            guild2ById.Result.Guild.Users.Should().HaveCount(2);
            guild3ById.Result.Guild.Users.Should().HaveCount(1);
        }

        [Test]
        public async Task AddUserToGuildWithMechBoardBuilding_WhenUserAdded_ThenChangeAttributesForEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await RemoveUserFromGuildByOwnerAsync(userId, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
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
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);
            await LeaveGuildAsync(userToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

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
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int user1Id, string user1Token) = await GetNewTokenAsync();
            (int user2Id, string user2Token) = await GetNewTokenAsync();
            (int user3Id, string user3Token) = await GetNewTokenAsync();

            GameUser user1 = await mayhemDataContext.GameUsers.Where(x => x.Id == user1Id).SingleOrDefaultAsync();
            user1.Npcs = new List<Npc>()
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
                };

            GameUser user2 = await mayhemDataContext.GameUsers.Where(x => x.Id == user2Id).SingleOrDefaultAsync();
            user2.Npcs = new List<Npc>()
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
                };
            GameUser user3 = await mayhemDataContext.GameUsers.Where(x => x.Id == user3Id).SingleOrDefaultAsync();
            user3.Npcs = new List<Npc>()
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
                };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request1 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
            };
            AddGuildBuildingCommandRequest request2 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };
            AddGuildBuildingCommandRequest request3 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };
            AddGuildBuildingCommandRequest request4 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };
            AddGuildBuildingCommandRequest request5 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request1, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request2, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request3, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request4, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request5, ownerToken);

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation1 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user1Token);
            await AcceptInvitationByOwnerAsync(invitation1.Result.InviteUser.Invitation.Id, ownerToken);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation2 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user2Token);
            await AcceptInvitationByOwnerAsync(invitation2.Result.InviteUser.Invitation.Id, ownerToken);
            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation3 = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, user3Token);
            await AcceptInvitationByOwnerAsync(invitation3.Result.InviteUser.Invitation.Id, ownerToken);

            await CloseGuildAsync(ownerToken);

            GameUser user1Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user1Id);

            GameUser user2Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user2Id);

            GameUser user3Db = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user3Id);

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
