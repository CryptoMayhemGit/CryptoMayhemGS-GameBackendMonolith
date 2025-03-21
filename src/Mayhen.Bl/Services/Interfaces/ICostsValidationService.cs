using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Util.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Costs Validation Service
    /// </summary>
    public interface ICostsValidationService
    {
        /// <summary>
        /// Validates the guild asynchronous.
        /// </summary>
        /// <param name="costs">The costs.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<ValidationMessage>> ValidateGuildAsync(Dictionary<ResourcesType, int> costs, int userId);

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="costs">The costs.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<ValidationMessage>> ValidateUserAsync(Dictionary<ResourcesType, int> costs, int userId);
    }
}