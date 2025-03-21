using Mayhem.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Explore Mission Repository
    /// </summary>
    public interface IExploreMissionRepository
    {
        /// <summary>
        /// Explores the mission asynchronous.
        /// </summary>
        /// <param name="exploreMissionDto">The explore mission dto.</param>
        /// <returns></returns>
        Task<ExploreMissionDto> ExploreMissionAsync(ExploreMissionDto exploreMissionDto);
    }
}
