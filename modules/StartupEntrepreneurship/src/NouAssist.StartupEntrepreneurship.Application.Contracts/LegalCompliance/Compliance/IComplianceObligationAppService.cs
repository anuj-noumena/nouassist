using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance
{
    public interface IComplianceObligationAppService : IApplicationService
    {
        Task<ComplianceObligationDto> GetAsync(Guid id);
        
        Task<PagedResultDto<ComplianceObligationListDto>> GetListAsync(GetComplianceObligationListDto input);
        
        Task<ComplianceObligationDto> CreateAsync(CreateComplianceObligationDto input);
        
        Task<ComplianceObligationDto> UpdateAsync(Guid id, UpdateComplianceObligationDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<ComplianceObligationDto> UpdateStatusAsync(Guid id, UpdateComplianceStatusDto input);
        
        Task SetDocumentUrlAsync(Guid id, string documentUrl);
        
        Task<List<ComplianceObligationListDto>> GetUpcomingObligationsAsync(int daysThreshold = 30);
        
        Task<List<ComplianceObligationListDto>> GetOverdueObligationsAsync();
        
        Task<int> GenerateRecurringObligationsAsync();
    }
}
