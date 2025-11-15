using System;
using System.Threading.Tasks;
using NouAssist.AuditTrail.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.AuditTrail
{
    public interface IAuditTrailAppService : IApplicationService
    {
        Task<AuditEntryDto> GetAsync(Guid id);
        
        Task<PagedResultDto<AuditEntryDto>> SearchAsync(AuditSearchDto input);
        
        Task<EntityAuditHistoryDto> GetEntityHistoryAsync(string entityType, string entityId);
        
        Task<AuditTrailReportDto> GenerateReportAsync(AuditSearchDto input);
        
        Task<int> ArchiveOldEntriesAsync();

        Task<AuditStatisticsDto> GetStatisticsAsync();
    }
}
