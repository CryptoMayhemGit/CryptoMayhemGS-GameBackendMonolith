using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Auth Service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Creates the token asynchronous.
        /// </summary>
        /// <param name="walletAddress">The wallet address.</param>
        /// <returns></returns>
        Task<string> CreateTokenAsync(string walletAddress);
        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<string> RefreshToken(int userId);
    }
}