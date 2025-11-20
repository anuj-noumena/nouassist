using NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest
{
    /// <summary>
    /// Application service interface for change request management
    /// </summary>
    public interface IChangeRequestAppService : IApplicationService
    {
        // Basic CRUD operations
        Task<ChangeRequestDto> GetAsync(Guid id);
        Task<PagedResultDto<ChangeRequestDto>> GetListAsync(GetChangeRequestListDto input);
        Task<ChangeRequestDto> CreateAsync(CreateChangeRequestDto input);
        Task<ChangeRequestDto> UpdateAsync(Guid id, UpdateChangeRequestDto input);
        Task DeleteAsync(Guid id);

        // Specific operations for change requests
        Task<ChangeRequestDto> AddRationaleAsync(Guid id, AddChangeRationaleDto input);
        Task RemoveRationaleAsync(Guid id, Guid rationaleId);
        Task<ChangeRequestDto> AddImpactStatementAsync(Guid id, AddImpactStatementDto input);
        Task RemoveImpactStatementAsync(Guid id, Guid impactStatementId);

        // Status management
        Task<ChangeRequestDto> ChangeStatusAsync(Guid id, ChangeStatusDto input);
        
        // Approval operations
        Task<ChangeRequestDto> SubmitApprovalDecisionAsync(Guid id, ApprovalDecisionDto input);
        Task<ChangeRequestDto> StartApprovalProcessAsync(Guid id);
        
        // Dashboard and filtering
        Task<List<ChangeRequestDto>> GetSubmittedByMeAsync();
        Task<List<ChangeRequestDto>> GetPendingMyApprovalAsync();
        Task<List<ChangeRequestDto>> GetHighImpactChangesAsync();
        Task<List<ChangeRequestDto>> GetRecentChangesAsync(int maxResultCount = 10);
        
        // Impact assessment
        Task<ImpactAssessmentResultDto> ConductImpactAssessmentAsync(Guid id);
    }
}
