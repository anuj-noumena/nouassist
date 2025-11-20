using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords
{
    public interface ICorporateRecordAppService : IApplicationService
    {
        Task<CorporateRecordDto> GetAsync(Guid id);
        
        Task<PagedResultDto<CorporateRecordListDto>> GetListAsync(GetCorporateRecordListDto input);
        
        Task<CorporateRecordDto> CreateAsync(CreateCorporateRecordDto input);
        
        Task<CorporateRecordDto> UpdateAsync(Guid id, UpdateCorporateRecordDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<CorporateRecordDto> UpdateStatusAsync(Guid id, FilingStatusType newStatus, string notes = null);
        
        Task SetDocumentUrlAsync(Guid id, string documentUrl);
        
        Task<List<CorporateRecordListDto>> GetRecordsByTypeAsync(CorporateRecordType type);
        
        Task<List<CorporateRecordListDto>> GetRecordsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        Task<List<CorporateRecordListDto>> GetRequiresFilingRecordsAsync();
    }
}
