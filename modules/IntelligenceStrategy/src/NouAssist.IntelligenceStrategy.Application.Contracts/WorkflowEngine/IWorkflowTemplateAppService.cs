using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Application service for workflow templates
    /// </summary>
    public interface IWorkflowTemplateAppService : IApplicationService
    {
        /// <summary>
        /// Gets a workflow template by ID
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <returns>The workflow template</returns>
        Task<WorkflowTemplateDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of workflow templates
        /// </summary>
        /// <param name="input">The filtering and paging parameters</param>
        /// <returns>The list of workflow templates</returns>
        Task<PagedResultDto<WorkflowTemplateDto>> GetListAsync(GetWorkflowTemplateListDto input);
        
        /// <summary>
        /// Creates a new workflow template
        /// </summary>
        /// <param name="input">The data for the new workflow template</param>
        /// <returns>The created workflow template</returns>
        Task<WorkflowTemplateDto> CreateAsync(CreateWorkflowTemplateDto input);
        
        /// <summary>
        /// Updates a workflow template
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <param name="input">The new data for the workflow template</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplateDto> UpdateAsync(Guid id, UpdateWorkflowTemplateDto input);
        
        /// <summary>
        /// Deletes a workflow template
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Updates the BPMN diagram of a workflow template
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <param name="input">The new BPMN data</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplateDto> UpdateBpmnAsync(Guid id, UpdateTemplateBpmnDto input);
        
        /// <summary>
        /// Updates the tags of a workflow template
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <param name="input">The new tags</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplateDto> UpdateTagsAsync(Guid id, UpdateTemplateTagsDto input);
        
        /// <summary>
        /// Makes a workflow template public (available to all tenants)
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplateDto> MakePublicAsync(Guid id);
        
        /// <summary>
        /// Makes a workflow template private (available only to its tenant)
        /// </summary>
        /// <param name="id">The ID of the workflow template</param>
        /// <returns>The updated workflow template</returns>
        Task<WorkflowTemplateDto> MakePrivateAsync(Guid id);
        
        /// <summary>
        /// Creates a workflow definition from a template
        /// </summary>
        /// <param name="id">The ID of the template</param>
        /// <param name="input">The data for the new workflow definition</param>
        /// <returns>The created workflow definition</returns>
        Task<WorkflowDefinitionDto> CreateFromTemplateAsync(Guid id, CreateFromTemplateDto input);
    }
}
