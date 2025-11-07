using System;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Workflows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Workflows
{
    public interface IWorkflowSchemeAppService : IApplicationService
    {
        Task<WorkflowSchemeDto> GetAsync(Guid id);
        Task<WorkflowSchemeDto> GetWithMappingsAsync(Guid id);
        Task<PagedResultDto<WorkflowSchemeDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<WorkflowSchemeDto> CreateAsync(CreateWorkflowSchemeDto input);
        Task DeleteAsync(Guid id);

        Task<WorkflowSchemeDto> PublishAsync(Guid id);
        Task<WorkflowSchemeDto> ActivateAsync(Guid id);
        Task<WorkflowSchemeDto> DeactivateAsync(Guid id);

        Task<WorkflowSchemeDto> GetActiveSchemeForProjectAsync(Guid projectId);
        Task<Guid> GetWorkflowForTaskTypeAsync(Guid schemeId, Guid taskTypeId);
    }
}
