using Mayhem.Worker.Dal.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Travel Repository
    /// </summary>
    public interface ITravelRepository
    {
        /// <summary>
        /// Gets the travels asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TravelDto>> GetTravelsAsync();
        /// <summary>
        /// Removes the travel asynchronous.
        /// </summary>
        /// <param name="travelId">The travel identifier.</param>
        /// <returns></returns>
        Task RemoveTravelAsync(long travelId);
        /// <summary>
        /// Removes the travels by NPC identifier asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        Task RemoveTravelsByNpcIdAsync(long npcId);
        /// <summary>
        /// Gets the travels by NPC identifier asynchronous.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TravelDto>> GetTravelsByNpcIdAsync(long npcId);
        /// <summary>
        /// Adds the travels asynchronous.
        /// </summary>
        /// <param name="travels">The travels.</param>
        /// <returns></returns>
        Task AddTravelsAsync(List<TravelDto> travels);
    }
}
