using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects
{
    /// <summary>
    /// Application service for managing defects
    /// </summary>
    public interface IDefectAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new defect report
        /// </summary>
        Task<DefectDto> CreateAsync(CreateDefectDto input);
        
        /// <summary>
        /// Updates an existing defect report
        /// </summary>
        Task<DefectDto> UpdateAsync(Guid id, UpdateDefectDto input);
        
        /// <summary>
        /// Deletes a defect report
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Gets a defect report by ID
        /// </summary>
        Task<DefectDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a defect report with all details including comments
        /// </summary>
        Task<DefectDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of defect reports
        /// </summary>
        Task<PagedResultDto<DefectDto>> GetListAsync(GetDefectListDto input);
        
        /// <summary>
        /// Gets defect reports for a specific test cycle
        /// </summary>
        Task<List<DefectDto>> GetByTestCycleIdAsync(Guid testCycleId);
        
        /// <summary>
        /// Gets defect reports for a specific test plan
        /// </summary>
        Task<List<DefectDto>> GetByTestPlanIdAsync(Guid testPlanId);
        
        /// <summary>
        /// Gets defect reports assigned to the current user
        /// </summary>
        Task<List<DefectDto>> GetMyDefectsAsync();
        
        /// <summary>
        /// Gets defect reports by status
        /// </summary>
        Task<List<DefectDto>> GetByStatusAsync(DefectStatus status);
        
        /// <summary>
        /// Gets high priority defect reports
        /// </summary>
        Task<List<DefectDto>> GetHighPriorityDefectsAsync();
        
        /// <summary>
        /// Assigns a defect to a user
        /// </summary>
        Task<DefectDto> AssignAsync(Guid id, AssignDefectDto input);
        
        /// <summary>
        /// Marks a defect as in progress
        /// </summary>
        Task<DefectDto> MarkAsInProgressAsync(Guid id);
        
        /// <summary>
        /// Marks a defect as fixed
        /// </summary>
        Task<DefectDto> MarkAsFixedAsync(Guid id, MarkDefectAsFixedDto input);
        
        /// <summary>
        /// Verifies a defect fix
        /// </summary>
        Task<DefectDto> VerifyFixAsync(Guid id, VerifyDefectFixDto input);
        
        /// <summary>
        /// Rejects a defect fix
        /// </summary>
        Task<DefectDto> RejectFixAsync(Guid id, string reason);
        
        /// <summary>
        /// Closes a defect
        /// </summary>
        Task<DefectDto> CloseAsync(Guid id);
        
        /// <summary>
        /// Reopens a defect
        /// </summary>
        Task<DefectDto> ReopenAsync(Guid id, string reason);
        
        /// <summary>
        /// Adds a comment to a defect
        /// </summary>
        Task<DefectCommentDto> AddCommentAsync(AddDefectCommentDto input);
        
        /// <summary>
        /// Links a requirement to a defect
        /// </summary>
        Task LinkRequirementAsync(Guid id, Guid requirementId);
        
        /// <summary>
        /// Unlinks a requirement from a defect
        /// </summary>
        Task UnlinkRequirementAsync(Guid id, Guid requirementId);
    }
}
