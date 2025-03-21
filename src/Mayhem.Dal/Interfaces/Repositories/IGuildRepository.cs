using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Guild Repository
    /// </summary>
    public interface IGuildRepository
    {
        /// <summary>
        /// Gets the guild by identifier asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        Task<GuildDto> GetGuildByIdAsync(int guildId);
        /// <summary>
        /// Gets the guild with resources by identifier asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        Task<GuildDto> GetGuildWithResourcesByIdAsync(int guildId);
        /// <summary>
        /// Gets the guild with resources by owner identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<GuildDto> GetGuildWithResourcesByOwnerIdAsync(int userId);
        /// <summary>
        /// Accepts the invitation by owner asynchronous.
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<AddUserToGuildDto> AcceptInvitationByOwnerAsync(int invitationId, int userId);
        /// <summary>
        /// Gets the guilds asynchronous.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<IEnumerable<GuildDto>> GetGuildsAsync(int? skip, int? limit, string name);
        /// <summary>
        /// Gets the invitations by guild identifier asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<GuildInvitationDto>> GetInvitationsByGuildIdAsync(int guildId, int userId);
        /// <summary>
        /// Gets the invitations by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<GuildInvitationDto>> GetInvitationsByUserIdAsync(int userId);
        /// <summary>
        /// Accepts the invitation by user asynchronous.
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<AddUserToGuildDto> AcceptInvitationByUserAsync(int invitationId, int userId);
        /// <summary>
        /// Adds the user to guild asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<AddUserToGuildDto> AddUserToGuildAsync(int guildId, int userId);
        /// <summary>
        /// Asks to join guild by user asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<InviteUserDto> AskToJoinGuildByUserAsync(int guildId, int userId);
        /// <summary>
        /// Changes the guild owner asynchronous.
        /// </summary>
        /// <param name="newOwnerId">The new owner identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> ChangeGuildOwnerAsync(int newOwnerId, int userId);
        /// <summary>
        /// Closes the guild asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> CloseGuildAsync(int userId);
        /// <summary>
        /// Creates the guild asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<GuildDto> CreateGuildAsync(string name, string description, int userId);
        /// <summary>
        /// Declines the invitation by owner asynchronous.
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns></returns>
        Task<bool> DeclineInvitationByOwnerAsync(int invitationId);
        /// <summary>
        /// Declines the invitation by user asynchronous.
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns></returns>
        Task<bool> DeclineInvitationByUserAsync(int invitationId);
        /// <summary>
        /// Invites the user by guild owner asynchronous.
        /// </summary>
        /// <param name="invitedUserId">The invited user identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<InviteUserDto> InviteUserByGuildOwnerAsync(int invitedUserId, int userId);
        /// <summary>
        /// Leaves the guild asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> LeaveGuildAsync(int userId);
        /// <summary>
        /// Removes the user from guild by owner asynchronous.
        /// </summary>
        /// <param name="removedUserId">The removed user identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveUserFromGuildByOwnerAsync(int removedUserId, int userId);
    }
}