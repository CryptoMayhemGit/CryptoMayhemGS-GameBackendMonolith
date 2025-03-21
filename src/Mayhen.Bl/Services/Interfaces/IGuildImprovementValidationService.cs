using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Guild Improvement Validation Service
    /// </summary>
    public interface IGuildImprovementValidationService
    {
        /// <summary>
        /// Validates the improvement.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="guildBuildingsType">Type of the guild buildings.</param>
        /// <param name="guildImprovements">The guild improvements.</param>
        /// <returns></returns>
        bool ValidateImprovement(int level, GuildBuildingsType guildBuildingsType, IEnumerable<GuildImprovementsType> guildImprovements);
    }
}