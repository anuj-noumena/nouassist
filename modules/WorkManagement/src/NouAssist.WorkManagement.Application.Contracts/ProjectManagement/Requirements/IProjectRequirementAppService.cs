using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Requirements.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Requirements
{
    public interface IProjectRequirementAppService : IApplicationService
    {
        Task<ProjectRequirementDto> GetAsync(Guid id);
        
        Task<List<ProjectRequirementDto>> GetByProjectAsync(Guid projectId);
        
        Task<List<ProjectRequirementDto>> GetPendingApprovalAsync();
        
        Task<ProjectRequirementDto> CreateAsync(CreateRequirementDto input);
        
        Task<ProjectRequirementDto> UpdateAsync(Guid id, UpdateRequirementDto input);
        
        Task DeleteAsync(Guid id);
    }
}
