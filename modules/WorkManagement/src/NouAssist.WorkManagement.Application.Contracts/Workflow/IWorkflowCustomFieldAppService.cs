using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow
{
    public interface IWorkflowCustomFieldAppService : IApplicationService
    {
        Task<WorkflowCustomFieldDto> GetAsync(Guid id);
        Task<List<WorkflowCustomFieldDto>> GetByWorkflowSchemeIdAsync(Guid workflowSchemeId);
        Task<List<WorkflowCustomFieldDto>> GetVisibleInStatusAsync(Guid workflowSchemeId, Guid statusId);
        Task<WorkflowCustomFieldDto> CreateAsync(CreateWorkflowCustomFieldDto input);
        Task<WorkflowCustomFieldDto> UpdateAsync(Guid id, UpdateWorkflowCustomFieldDto input);
        Task DeleteAsync(Guid id);
        Task<WorkflowCustomFieldDto> ActivateAsync(Guid id);
        Task<WorkflowCustomFieldDto> DeactivateAsync(Guid id);
    }
}