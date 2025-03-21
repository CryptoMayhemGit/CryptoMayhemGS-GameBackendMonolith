using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.AuditLogs;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public AuditLogRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<AuditLogDto> AddAuditLogAsync(AuditLogDto auditLogDto)
        {
            EntityEntry<AuditLog> auditLog = await mayhemDataContext.AuditLogs.AddAsync(mapper.Map<AuditLog>(auditLogDto));
            await mayhemDataContext.SaveChangesAsync();

            return mapper.Map<AuditLogDto>(auditLog.Entity);
        }
    }
}
