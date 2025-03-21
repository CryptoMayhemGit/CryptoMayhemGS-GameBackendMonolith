using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// User Land Repository
    /// </summary>
    public interface IUserLandRepository
    {
        /// <summary>
        /// Gets the land asynchronous.
        /// </summary>
        /// <param name="userLandId">The land identifier.</param>
        /// <returns></returns>
        Task<UserLandDto> GetUserLandAsync(long userLandId);
        /// <summary>
        /// Gets the user lands asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<UserLandDto>> GetUserLandsAsync(int userId);
        /// <summary>
        /// Checks the purchase land asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<bool> CheckPurchaseLandAsync(long landId, int userId);

        /// <summary>
        /// Discovers the land asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<UserLandDto> DiscoverUserLandAsync(long landId, int userId);
        /// <summary>
        /// Explores the user land asynchronous.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<UserLandDto> ExploreUserLandAsync(long landId, int userId);
        /// <summary>
        /// Get not discovered lands from user perspective
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="landInstanceId">The land instance identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<LandPositionDto>> GetUserLandsFromUserPerspectiveAsync(int userId, int landInstanceId);
        /// <summary>
        /// Gets the enemy user lands asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="landIds">The land ids.</param>
        /// <returns></returns>
        Task<IEnumerable<UserLandDto>> GetEnemyUserLandsAsync(int userId, IEnumerable<long> landIds);
    }
}