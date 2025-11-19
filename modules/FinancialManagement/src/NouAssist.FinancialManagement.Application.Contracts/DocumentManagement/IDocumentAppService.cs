using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.DocumentManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.DocumentManagement
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentDto> GetAsync(Guid id);
        
        Task<PagedResultDto<DocumentDto>> SearchAsync(DocumentSearchDto input);
        
        Task<ListResultDto<DocumentDto>> GetByEntityAsync(string entityType, string entityId);
        
        Task<DocumentDto> UploadAsync(CreateDocumentDto input);
        
        Task<DocumentDto> UpdateAsync(Guid id, UpdateDocumentDto input);
        
        Task<DocumentDownloadDto> DownloadAsync(Guid id);
        
        Task DeleteAsync(Guid id, string reason = null);
        
        Task ArchiveAsync(Guid id);
        
        Task<int> ProcessPendingOCRAsync();

        Task<DocumentStatisticsDto> GetStatisticsAsync();
    }
}
