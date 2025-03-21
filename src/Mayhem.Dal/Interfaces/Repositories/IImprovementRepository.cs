using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Improvement Repository
    /// </summary>
    public interface IImprovementRepository
    {
        /// <summary>
        /// Adds the improvement asynchronous.
        /// </summary>
        /// <param name="improvementDto">The improvement dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<ImprovementDto> AddImprovementAsync(ImprovementDto improvementDto, int userId);
        /// <summary>
        /// Adds the guild improvement asynchronous.
        /// </summary>
        /// <param name="guildImprovementDto">The guild improvement dto.</param>
        /// <returns></returns>
        Task<GuildImprovementDto> AddGuildImprovementAsync(GuildImprovementDto guildImprovementDto);
        /// <summary>
        /// Gets the improvement by land identifier asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ImprovementDto>> GetImprovementsByLandIdAsync(long landId);
        /// <summary>
        /// Gets the guild improvements by guild identifier asynchronous.
        /// </summary>
        /// <param name="guildId">The guild identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<GuildImprovementDto>> GetGuildImprovementsByGuildIdAsync(long guildId);
    }
}