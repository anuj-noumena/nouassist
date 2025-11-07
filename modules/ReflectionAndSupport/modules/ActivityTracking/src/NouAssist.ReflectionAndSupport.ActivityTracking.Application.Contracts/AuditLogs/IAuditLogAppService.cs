using System;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.ActivityTracking.AuditLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<AuditLogDto> GetAsync(Guid id);
        Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListDto input);
    }
}
