using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// User Repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates the user asynchronous.
        /// </summary>
        /// <param name="walletAddress">The wallet address.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<int?> CreateUserAsync(string walletAddress, string email);
        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="walletAddress">The wallet address.</param>
        /// <returns></returns>
        Task LoginAsync(string walletAddress);
        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="getUserRequest">The get user request.</param>
        /// <returns></returns>
        Task<GetUserCommandResponseDto> GetUserAsync(GetUserCommandRequestDto getUserRequest);
        /// <summary>
        /// Gets the application user by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetApplicationUserByIdAsync(int userId);
        /// <summary>
        /// Gets the application user by wallet asynchronous.
        /// </summary>
        /// <param name="wallet">The wallet.</param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetApplicationUserByWalletAsync(string wallet);
        /// <summary>
        /// Checks the email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> CheckEmailAsync(string email);
    }
}