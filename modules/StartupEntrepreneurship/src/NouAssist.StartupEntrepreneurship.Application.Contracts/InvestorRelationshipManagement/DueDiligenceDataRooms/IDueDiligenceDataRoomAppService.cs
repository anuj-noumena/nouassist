using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms
{
    /// <summary>
    /// Application service interface for due diligence data room management
    /// </summary>
    public interface IDueDiligenceDataRoomAppService : IApplicationService
    {
        // CRUD Operations
        Task<DueDiligenceDataRoomDto> GetAsync(Guid id);
        Task<DueDiligenceDataRoomDto> GetWithDetailsAsync(Guid id);
        Task<PagedResultDto<DueDiligenceDataRoomDto>> GetListAsync(GetDueDiligenceDataRoomListDto input);
        Task<PagedResultDto<DueDiligenceDataRoomDto>> GetAccessibleAsync(PagedAndSortedResultRequestDto input);
        Task<DueDiligenceDataRoomDto> CreateAsync(CreateDueDiligenceDataRoomDto input);
        Task<DueDiligenceDataRoomDto> UpdateAsync(Guid id, UpdateDueDiligenceDataRoomDto input);
        Task DeleteAsync(Guid id);

        // Access Management
        Task GrantAccessAsync(Guid id, GrantAccessDto input);
        Task RevokeAccessAsync(Guid id, RevokeAccessDto input);
        Task<PagedResultDto<DataRoomInvestorAccessDto>> GetInvestorAccessAsync(Guid id, PagedAndSortedResultRequestDto input);
        Task<bool> CanInvestorAccessAsync(Guid id, Guid investorId);

        // Document Management
        Task<PagedResultDto<SecureDocumentDto>> GetDocumentsAsync(Guid id, PagedAndSortedResultRequestDto input);
        Task<SecureDocumentDto> UploadDocumentAsync(Guid id, UploadDocumentDto input /* + file data */);
        Task<SecureDocumentDto> UpdateDocumentAsync(Guid id, Guid documentId, UpdateDocumentDto input);
        Task DeleteDocumentAsync(Guid id, Guid documentId);

        // Secure Sharing
        Task<SecureShareLinkDto> GenerateShareLinkAsync(Guid id, GenerateShareLinkDto input);
        Task LogDocumentAccessAsync(Guid id, Guid documentId, string action);

        // Analytics
        Task<PagedResultDto<DocumentAccessSummaryDto>> GetDocumentAccessReportAsync(Guid id, GenerateReportDto input);

        // Status Management
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
    }
}
