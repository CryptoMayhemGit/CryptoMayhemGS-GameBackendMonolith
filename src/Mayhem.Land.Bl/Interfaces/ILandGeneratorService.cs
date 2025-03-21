using Mayhem.Dal.Dto.Dtos;
using Mayhem.Land.Bl.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Land.Bl.Interfaces
{
    /// <summary>
    /// Land Generator Service
    /// </summary>
    public interface ILandGeneratorService
    {
        /// <summary>
        /// Reads the lands asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<ImportLandDto>> ReadLandsAsync();
        /// <summary>
        /// Saves the lands asynchronous.
        /// </summary>
        /// <param name="lands">The lands.</param>
        /// <returns></returns>
        Task<IEnumerable<LandDto>> SaveLandsAsync(List<ImportLandDto> lands);
    }
}
