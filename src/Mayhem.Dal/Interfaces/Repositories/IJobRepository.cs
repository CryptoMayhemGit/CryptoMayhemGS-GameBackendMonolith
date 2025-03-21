using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Job Repository
    /// </summary>
    public interface IJobRepository
    {
        /// <summary>
        /// Gets the jobs by land identifier asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<JobDto>> GetJobsByLandIdAsync(long landId);
    }
}