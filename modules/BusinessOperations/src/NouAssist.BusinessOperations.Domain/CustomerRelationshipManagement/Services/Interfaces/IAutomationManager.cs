namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing automation workflows
    /// </summary>
    public interface IAutomationManager
    {
        /// <summary>
        /// Creates a new automation workflow
        /// </summary>
        Task<AutomationWorkflow> CreateWorkflowAsync(
            string name,
            string description,
            AutomationTriggerType triggerType,
            string entityType,
            string fieldName = null,
            string expectedValue = null,
            string scheduleExpression = null,
            bool activate = false);
            
        /// <summary>
        /// Updates an existing workflow
        /// </summary>
        Task<AutomationWorkflow> UpdateWorkflowAsync(
            Guid id,
            string name = null,
            string description = null);
            
        /// <summary>
        /// Updates the trigger condition of a workflow
        /// </summary>
        Task<AutomationWorkflow> UpdateTriggerConditionAsync(
            Guid id,
            AutomationTriggerType? triggerType = null,
            string entityType = null,
            string fieldName = null,
            string expectedValue = null,
            string scheduleExpression = null);
            
        /// <summary>
        /// Activates a workflow
        /// </summary>
        Task<AutomationWorkflow> ActivateWorkflowAsync(
            Guid id);
            
        /// <summary>
        /// Deactivates a workflow
        /// </summary>
        Task<AutomationWorkflow> DeactivateWorkflowAsync(
            Guid id);
            
        /// <summary>
        /// Adds a step to a workflow
        /// </summary>
        Task<WorkflowStep> AddWorkflowStepAsync(
            Guid workflowId,
            string name,
            string description,
            AutomationActionType actionType,
            string actionConfiguration,
            TimeSpan? delayBefore = null);
            
        /// <summary>
        /// Updates a workflow step
        /// </summary>
        Task<WorkflowStep> UpdateWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step,
            string name = null,
            string description = null,
            AutomationActionType? actionType = null,
            string actionConfiguration = null,
            TimeSpan? delayBefore = null);
            
        /// <summary>
        /// Reorders the steps in a workflow
        /// </summary>
        Task ReorderWorkflowStepsAsync(
            Guid workflowId,
            List<(WorkflowStep Step, int NewOrder)> newOrdering);
            
        /// <summary>
        /// Enables a workflow step
        /// </summary>
        Task<WorkflowStep> EnableWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step);
            
        /// <summary>
        /// Disables a workflow step
        /// </summary>
        Task<WorkflowStep> DisableWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step);
            
        /// <summary>
        /// Removes a step from a workflow
        /// </summary>
        Task RemoveWorkflowStepAsync(
            Guid workflowId,
            WorkflowStep step);
            
        /// <summary>
        /// Triggers a workflow execution
        /// </summary>
        Task<WorkflowExecutionRecord> TriggerWorkflowExecutionAsync(
            Guid workflowId,
            Guid triggeredByEntityId,
            string triggeredByEntityType);
            
        /// <summary>
        /// Gets workflows that should be triggered for an event
        /// </summary>
        Task<List<AutomationWorkflow>> GetMatchingWorkflowsAsync(
            AutomationTriggerType triggerType,
            string entityType,
            string fieldName = null,
            string fieldValue = null);
    }
}
