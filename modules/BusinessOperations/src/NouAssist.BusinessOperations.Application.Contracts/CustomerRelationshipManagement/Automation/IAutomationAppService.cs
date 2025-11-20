using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Automation.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Automation
{
    /// <summary>
    /// Application service for automation workflow management
    /// </summary>
    public interface IAutomationAppService : IApplicationService
    {
        /// <summary>
        /// Gets an automation workflow by ID
        /// </summary>
        Task<AutomationWorkflowDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of automation workflows with filtering and paging
        /// </summary>
        Task<PagedResultDto<AutomationWorkflowDto>> GetListAsync(GetAutomationWorkflowListDto input);
        
        /// <summary>
        /// Creates a new automation workflow
        /// </summary>
        Task<AutomationWorkflowDto> CreateAsync(CreateAutomationWorkflowDto input);
        
        /// <summary>
        /// Updates an existing automation workflow
        /// </summary>
        Task<AutomationWorkflowDto> UpdateAsync(Guid id, UpdateAutomationWorkflowDto input);
        
        /// <summary>
        /// Deletes an automation workflow
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Updates the trigger condition of a workflow
        /// </summary>
        Task<AutomationWorkflowDto> UpdateTriggerConditionAsync(Guid id, UpdateAutomationTriggerConditionDto input);
        
        /// <summary>
        /// Activates a workflow
        /// </summary>
        Task<AutomationWorkflowDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Deactivates a workflow
        /// </summary>
        Task<AutomationWorkflowDto> DeactivateAsync(Guid id);
        
        /// <summary>
        /// Adds a step to a workflow
        /// </summary>
        Task<WorkflowStepDto> AddStepAsync(Guid workflowId, CreateWorkflowStepDto input);
        
        /// <summary>
        /// Updates a workflow step
        /// </summary>
        Task<WorkflowStepDto> UpdateStepAsync(Guid workflowId, Guid stepId, UpdateWorkflowStepDto input);
        
        /// <summary>
        /// Reorders the steps in a workflow
        /// </summary>
        Task ReorderStepsAsync(Guid workflowId, ReorderWorkflowStepsDto input);
        
        /// <summary>
        /// Enables a workflow step
        /// </summary>
        Task<WorkflowStepDto> EnableStepAsync(Guid workflowId, Guid stepId);
        
        /// <summary>
        /// Disables a workflow step
        /// </summary>
        Task<WorkflowStepDto> DisableStepAsync(Guid workflowId, Guid stepId);
        
        /// <summary>
        /// Removes a step from a workflow
        /// </summary>
        Task RemoveStepAsync(Guid workflowId, Guid stepId);
        
        /// <summary>
        /// Triggers a workflow execution manually
        /// </summary>
        Task<WorkflowExecutionRecordDto> TriggerExecutionAsync(Guid workflowId, TriggerWorkflowDto input);
    }
}
