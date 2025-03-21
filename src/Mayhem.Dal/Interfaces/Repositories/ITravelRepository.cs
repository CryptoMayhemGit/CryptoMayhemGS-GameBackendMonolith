using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Travel Repository
    /// </summary>
    public interface ITravelRepository
    {
        /// <summary>
        /// Gets the travels from by land identifier asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TravelDto>> GetTravelsFromByLandIdAsync(long landId);
        /// <summary>
        /// Removes the travels by NPC identifier asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveTravelsByNpcIdAsync(long npcId);

        /// <summary>
        /// Gets the travels to by land identifier asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TravelDto>> GetTravelsToByLandIdAsync(long landId);
        /// <summary>
        /// Add the travels asynchronous.
        /// </summary>
        /// <param name="travelsDto">The travels dto.</param>
        /// <returns></returns>
        Task<IEnumerable<TravelDto>> AddTravelsAsync(IEnumerable<TravelDto> travelsDto);
    }
}