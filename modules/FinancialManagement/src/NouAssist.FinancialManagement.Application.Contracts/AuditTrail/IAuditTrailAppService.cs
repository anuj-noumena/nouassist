using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AuditTrail.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AuditTrail
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
