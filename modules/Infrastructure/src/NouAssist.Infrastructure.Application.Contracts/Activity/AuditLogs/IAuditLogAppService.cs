using System;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.AuditLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Activity.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<AuditLogDto> GetAsync(Guid id);
        Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListDto input);
    }
}
