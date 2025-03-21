using System.Threading.Tasks;
using Mayhem.SmtpBase.Dto;

namespace Mayhem.SmtpBase.Services.Interfaces
{
    /// <summary>
    /// Smtp Service
    /// </summary>
    public interface ISmtpService
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<bool> SendAsync(EmailRequestDto request);
    }
}
