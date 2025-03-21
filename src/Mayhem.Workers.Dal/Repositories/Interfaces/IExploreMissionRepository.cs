using Mayhem.Worker.Dal.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Explore Mission Repository
    /// </summary>
    public interface IExploreMissionRepository
    {
        /// <summary>
        /// Gets the finished missions asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ExploreMissionDto>> GetFinishedMissionsAsync();
        /// <summary>
        /// Removes the mission asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task RemoveMissionAsync(long id);
    }
}
