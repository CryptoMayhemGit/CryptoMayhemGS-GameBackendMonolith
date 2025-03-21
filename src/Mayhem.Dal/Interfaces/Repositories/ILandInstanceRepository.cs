using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Land Instance Repository
    /// </summary>
    public interface ILandInstanceRepository
    {
        /// <summary>
        /// Adds the instance asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<LandInstanceDto> AddInstanceAsync();
        /// <summary>
        /// Adds the instances asynchronous.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<IEnumerable<LandInstanceDto>> AddInstancesAsync(int count);
        /// <summary>
        /// Gets the last insance asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<LandInstanceDto> GetLastInsanceAsync();
    }
}