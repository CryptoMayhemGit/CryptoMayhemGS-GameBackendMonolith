using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Guild Building Repository
    /// </summary>
    public interface IGuildBuildingRepository
    {
        /// <summary>
        /// Adds the guild building asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <param name="guildBuildingsType">Type of the guild buildings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<GuildBuildingDto> AddGuildBuildingAsync(int guildId, GuildBuildingsType guildBuildingsType, int userId);
        /// <summary>
        /// Gets the guild buildings by guild identifier asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<GuildBuildingDto>> GetGuildBuildingsByGuildIdAsync(int guildId);
        /// <summary>
        /// Gets the guild building by identifier asynchronous.
        /// </summary>
        /// <param name="guildBuildingId">The guild building identifier.</param>
        /// <returns></returns>
        Task<GuildBuildingDto> GetGuildBuildingByIdAsync(int guildBuildingId);
        /// <summary>
        /// Upgrades the guild building asynchronous.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<GuildBuildingDto> UpgradeGuildBuildingAsync(int buildingId, int userId);
    }
}