using AutoMapper;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Helper;
using Mayhem.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class GuildRepository : IGuildRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        private static ICollection<GuildResource> BasicResource =>
            Enum.GetValues(typeof(ResourcesType)).Cast<ResourcesType>().Select(type => new GuildResource()
            {
                ResourceTypeId = type,
                Value = 0,
            }).ToList();

        public GuildRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GuildDto>> GetGuildsAsync(int? skip, int? limit, string name)
        {
            IQueryable<Guild> query = mayhemDataContext
                .Guilds
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (limit.HasValue && limit.Value > 50)
            {
                limit = 50;
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(name.ToUpper()));
            }
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (limit.HasValue && limit.Value > 0)
            {
                query = query.Take(limit.Value);
            }

            return await query.Select(x => mapper.Map<GuildDto>(x)).ToListAsync();
        }

        public async Task<GuildDto> GetGuildByIdAsync(int guildId) => await mayhemDataContext
                .Guilds
                .AsNoTracking()
                .Include(x => x.Users)
                .Where(x => x.Id == guildId)
                .Select(x => mapper.Map<GuildDto>(x))
                .SingleOrDefaultAsync();

        public async Task<GuildDto> GetGuildWithResourcesByIdAsync(int guildId) => await mayhemDataContext
                .Guilds
                .AsNoTracking()
                .Include(x => x.GuildResources)
                .Where(x => x.Id == guildId)
                .Select(x => mapper.Map<GuildDto>(x))
                .SingleOrDefaultAsync();

        public async Task<GuildDto> GetGuildWithResourcesByOwnerIdAsync(int userId) => await mayhemDataContext
                .Guilds
                .AsNoTracking()
                .Include(x => x.GuildResources)
                .Where(x => x.OwnerId == userId)
                .Select(x => mapper.Map<GuildDto>(x))
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<GuildInvitationDto>> GetInvitationsByGuildIdAsync(int guildId, int userId) => await mayhemDataContext
                .GuildInvitations
                .AsNoTracking()
                .Include(x => x.Guild)
                .Where(x => x.GuildId == guildId && x.InvitationType == GuildInvitationsType.UserSentInvitation && x.Guild.OwnerId == userId)
                .Select(x => mapper.Map<GuildInvitationDto>(x))
                .ToListAsync();

        public async Task<IEnumerable<GuildInvitationDto>> GetInvitationsByUserIdAsync(int userId) => await mayhemDataContext
                .GuildInvitations
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.InvitationType == GuildInvitationsType.UserBeenInvited)
                .Select(x => mapper.Map<GuildInvitationDto>(x))
                .ToListAsync();

        public async Task<GuildDto> CreateGuildAsync(string name, string description, int userId)
        {
            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);

            Guild guild = new()
            {
                Name = name,
                Description = description,
                OwnerId = userId,
                Users = new List<GameUser>()
                {
                    user
                }
            };

            guild.GuildResources = BasicResource;

            await mayhemDataContext.Guilds.AddAsync(guild);
            await mayhemDataContext.SaveChangesAsync();

            return mapper.Map<GuildDto>(guild);
        }

        public async Task<bool> CloseGuildAsync(int userId)
        {
            Guild guild = await mayhemDataContext
                .Guilds
                .Include(x => x.GuildInvitations)
                .Include(x => x.GuildBuildings)
                .ThenInclude(x => x.GuildBuildingBonuses)
                .Include(x => x.GuildImprovements)
                .Include(x => x.GuildResources)
                .Include(x => x.Users)
                .ThenInclude(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.OwnerId == userId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                foreach (GameUser user in guild.Users)
                {
                    user.GuildId = null;
                    UpdateUserNpcsAttributes(user, guild, false);
                }

                mayhemDataContext.GuildResources.RemoveRange(guild.GuildResources);
                mayhemDataContext.GuildInvitations.RemoveRange(guild.GuildInvitations);
                mayhemDataContext.GuildBuildingBonuses.RemoveRange(guild.GuildBuildings.SelectMany(x => x.GuildBuildingBonuses));
                mayhemDataContext.GuildBuildings.RemoveRange(guild.GuildBuildings);
                mayhemDataContext.GuildImprovements.RemoveRange(guild.GuildImprovements);
                mayhemDataContext.Guilds.Remove(guild);

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(CloseGuildAsync));
            }

            return true;
        }

        public async Task<InviteUserDto> InviteUserByGuildOwnerAsync(int invitedUserId, int userId)
        {
            Guild guild = await mayhemDataContext
                .Guilds
                .SingleOrDefaultAsync(x => x.OwnerId == userId);

            InviteUserDto inviteUserDto = new();

            GuildInvitation existingInvitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.UserId == invitedUserId && x.GuildId == guild.Id);

            if (existingInvitation == null)
            {
                GuildInvitation guildInvitation = new()
                {
                    UserId = invitedUserId,
                    GuildId = guild.Id,
                    InvitationType = GuildInvitationsType.UserBeenInvited,
                };
                EntityEntry<GuildInvitation> invitation = await mayhemDataContext.GuildInvitations.AddAsync(guildInvitation);
                await mayhemDataContext.SaveChangesAsync();

                inviteUserDto.Invitation = mapper.Map<GuildInvitationDto>(invitation.Entity);
            }
            else if (existingInvitation.InvitationType == GuildInvitationsType.UserSentInvitation)
            {
                inviteUserDto.AddedUserToGuild = await AddUserToGuildAsync(guild.Id, invitedUserId);
            }

            return inviteUserDto;
        }

        public async Task<InviteUserDto> AskToJoinGuildByUserAsync(int guildId, int userId)
        {
            InviteUserDto inviteUserDto = new();

            GuildInvitation existingInvitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.UserId == userId && x.GuildId == guildId);

            if (existingInvitation == null)
            {
                GuildInvitation guildInvitation = new()
                {
                    UserId = userId,
                    GuildId = guildId,
                    InvitationType = GuildInvitationsType.UserSentInvitation,
                };
                EntityEntry<GuildInvitation> invitation = await mayhemDataContext.GuildInvitations.AddAsync(guildInvitation);
                await mayhemDataContext.SaveChangesAsync();

                inviteUserDto.Invitation = mapper.Map<GuildInvitationDto>(invitation.Entity);
            }
            else if (existingInvitation.InvitationType == GuildInvitationsType.UserBeenInvited)
            {
                inviteUserDto.AddedUserToGuild = await AddUserToGuildAsync(guildId, userId);
            }

            return inviteUserDto;
        }

        public async Task<AddUserToGuildDto> AddUserToGuildAsync(int guildId, int userId)
        {
            Guild guild = await mayhemDataContext
                .Guilds
                .Include(x => x.Users)
                .Include(x => x.GuildBuildings)
                .ThenInclude(x => x.GuildBuildingBonuses)
                .SingleOrDefaultAsync(x => x.Id == guildId);

            GameUser user = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                guild.Users.Add(user);
                mayhemDataContext.GuildInvitations.RemoveRange(mayhemDataContext.GuildInvitations.Where(x => x.UserId == user.Id));

                UpdateUserNpcsAttributes(user, guild, true);

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddUserToGuildAsync));
            }

            return new AddUserToGuildDto()
            {
                Guild = mapper.Map<GuildDto>(guild),
                User = mapper.Map<GameUserDto>(user),
            };
        }

        public async Task<AddUserToGuildDto> AcceptInvitationByOwnerAsync(int invitationId, int userId)
        {
            GuildInvitation invitation = await mayhemDataContext
                .GuildInvitations
                .Include(x => x.Guild)
                .SingleOrDefaultAsync(x => x.Id == invitationId && x.InvitationType == GuildInvitationsType.UserSentInvitation);

            return await AddUserToGuildAsync(invitation.GuildId, invitation.UserId);
        }

        public async Task<bool> DeclineInvitationByOwnerAsync(int invitationId)
        {
            GuildInvitation invitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.Id == invitationId && x.InvitationType == GuildInvitationsType.UserSentInvitation);

            mayhemDataContext.GuildInvitations.Remove(invitation);
            await mayhemDataContext.SaveChangesAsync();
            return true;
        }

        public async Task<AddUserToGuildDto> AcceptInvitationByUserAsync(int invitationId, int userId)
        {
            GuildInvitation invitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.Id == invitationId && x.InvitationType == GuildInvitationsType.UserBeenInvited);

            return await AddUserToGuildAsync(invitation.GuildId, invitation.UserId);
        }

        public async Task<bool> DeclineInvitationByUserAsync(int invitationId)
        {
            GuildInvitation invitation = await mayhemDataContext
                .GuildInvitations
                .SingleOrDefaultAsync(x => x.Id == invitationId && x.InvitationType == GuildInvitationsType.UserBeenInvited);

            mayhemDataContext.GuildInvitations.Remove(invitation);
            await mayhemDataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromGuildByOwnerAsync(int removedUserId, int userId)
        {
            Guild guild = await mayhemDataContext
               .Guilds
               .Include(x => x.Users)
               .Include(x => x.GuildBuildings)
               .ThenInclude(x => x.GuildBuildingBonuses)
               .SingleOrDefaultAsync(x => x.OwnerId == userId);

            GameUser removedUser = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == removedUserId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                guild.Users.Remove(removedUser);
                UpdateUserNpcsAttributes(removedUser, guild, false);
                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(RemoveUserFromGuildByOwnerAsync));
            }
        }

        public async Task<bool> LeaveGuildAsync(int userId)
        {
            GameUser user = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .Include(x => x.GuildOwner)
                .Include(x => x.GuildMember)
                .ThenInclude(x => x.Users)
                .Include(x => x.GuildMember)
                .ThenInclude(x => x.GuildBuildings)
                .ThenInclude(x => x.GuildBuildingBonuses)
                .SingleOrDefaultAsync(x => x.Id == userId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                user.GuildMember.Users.Remove(user);
                UpdateUserNpcsAttributes(user, user.GuildMember, false);

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(LeaveGuildAsync));
            }
        }

        public async Task<bool> ChangeGuildOwnerAsync(int newOwnerId, int userId)
        {
            GameUser user = await mayhemDataContext
               .GameUsers
               .Include(x => x.GuildOwner)
               .ThenInclude(x => x.Users)
               .SingleOrDefaultAsync(x => x.Id == userId);

            user.GuildOwner.OwnerId = newOwnerId;
            await mayhemDataContext.SaveChangesAsync();
            return true;
        }

        private static void UpdateUserNpcsAttributes(GameUser user, Guild guild, bool increase)
        {
            foreach (Npc npc in user.Npcs)
            {
                foreach (GuildBuilding guildBuilding in guild.GuildBuildings)
                {
                    IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(guildBuilding.GuildBuildingTypeId);
                    List<Tables.Attribute> attributesToUpdate = npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId)).ToList();
                    foreach (Tables.Attribute attribute in attributesToUpdate)
                    {
                        if (increase)
                        {
                            attribute.Value = BonusHelper.IncreaseBonusValue(guildBuilding.GuildBuildingBonuses.First().Bonus, attribute.BaseValue, attribute.Value);
                        }
                        else
                        {
                            attribute.Value = BonusHelper.DecreaseBonusValue(guildBuilding.GuildBuildingBonuses.First().Bonus, attribute.BaseValue, attribute.Value);
                        }
                    }
                }
            }
        }
    }
}
