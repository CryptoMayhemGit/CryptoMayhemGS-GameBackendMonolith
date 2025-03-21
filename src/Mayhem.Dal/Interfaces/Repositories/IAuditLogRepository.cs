using Mayhem.Dal.Dto.Dtos;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Audit Log Repository
    /// </summary>
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Adds the audit log asynchronous.
        /// </summary>
        /// <param name="auditLogDto">The audit log.</param>
        /// <returns></returns>
        Task<AuditLogDto> AddAuditLogAsync(AuditLogDto auditLogDto);
    }
}
