using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Bonus Repository
    /// </summary>
    public interface IBonusRepository
    {
        /// <summary>
        /// Gets the mech bonus by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<double> GetMechBonusByUserIdAsync(int userId);
    }
}
