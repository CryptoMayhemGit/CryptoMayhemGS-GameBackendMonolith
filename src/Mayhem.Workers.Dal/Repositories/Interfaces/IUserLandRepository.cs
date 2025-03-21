using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Workers.Dal.Repositories.Interfaces
{
    /// <summary>
    /// User Land Repository
    /// </summary>
    public interface IUserLandRepository
    {
        /// <summary>
        /// Gets the user lands from user perspective asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<LandPositionDto>> GetUserLandsFromUserPerspectiveAsync(int userId);
        /// <summary>
        /// Gets the user land asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        Task<UserLandDto> GetUserLandAsync(int userId, long landId);
        /// <summary>
        /// Updates the user land status asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        Task UpdateUserLandStatusAsync(int id, LandsStatus status);
        /// <summary>
        /// Adds the user land asynchronous.
        /// </summary>
        /// <param name="userLand">The user land.</param>
        /// <returns></returns>
        Task AddUserLandAsync(UserLandDto userLand);
    }
}
