using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Improvement Validation Service
    /// </summary>
    public interface IImprovementValidationService
    {
        /// <summary>
        /// Validates the improvement.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="BuildingsType">Type of the buildings.</param>
        /// <param name="improvements">The improvements.</param>
        /// <returns></returns>
        bool ValidateImprovement(int level, BuildingsType BuildingsType, IEnumerable<ImprovementsType> improvements);
    }
}