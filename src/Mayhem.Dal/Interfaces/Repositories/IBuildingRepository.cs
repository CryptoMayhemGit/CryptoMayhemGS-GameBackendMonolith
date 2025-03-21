using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Building Repository
    /// </summary>
    public interface IBuildingRepository
    {
        /// <summary>
        /// Gets the buildings by land identifier asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<BuildingDto>> GetBuildingsByLandIdAsync(long landId);
        /// <summary>
        /// Adds the building to land asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="buildingType">Type of the building.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<BuildingDto> AddBuildingToLandAsync(long landId, BuildingsType buildingType, int userId);
        /// <summary>
        /// Upgrades the building asynchronous.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<BuildingDto> UpgradeBuildingAsync(long buildingId, int userId);
        /// <summary>
        /// Gets the building by identifier asynchronous.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns></returns>
        Task<BuildingDto> GetBuildingByIdAsync(long buildingId);
        /// <summary>
        /// Gets the land by building identifier asynchronous.
        /// </summary>
        /// <param name="buildingId">The identifier.</param>
        /// <returns></returns>
        Task<LandDto> GetLandByBuildingIdAsync(long buildingId);
        /// <summary>
        /// Gets the buildings by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<BuildingDto>> GetBuildingsByUserIdAsync(int userId);
        /// <summary>
        /// Gets the enemy buildings by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="landIds">The land ids.</param>
        /// <returns></returns>
        Task<IEnumerable<BuildingDto>> GetEnemyBuildingsByUserIdAsync(int userId, IEnumerable<long> landIds);
    }
}