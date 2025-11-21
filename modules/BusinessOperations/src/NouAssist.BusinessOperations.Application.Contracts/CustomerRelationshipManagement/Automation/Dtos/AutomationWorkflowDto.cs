using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Automation.Dtos
{
    /// <summary>
    /// Data transfer object for AutomationWorkflow entity
    /// </summary>
    public class AutomationWorkflowDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the workflow
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the workflow
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Whether this workflow is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Trigger condition for the workflow
        /// </summary>
        public AutomationTriggerConditionDto TriggerCondition { get; set; }
        
        /// <summary>
        /// Steps in the workflow
        /// </summary>
        public List<WorkflowStepDto> Steps { get; set; }
        
        /// <summary>
        /// Execution history of this workflow
        /// </summary>
        public List<WorkflowExecutionRecordDto> ExecutionRecords { get; set; }
    }

    /// <summary>
    /// Data transfer object for AutomationTriggerCondition value object
    /// </summary>
    public class AutomationTriggerConditionDto
    {
        /// <summary>
        /// The type of trigger
        /// </summary>
        public AutomationTriggerType TriggerType { get; set; }
        
        /// <summary>
        /// The entity type this trigger applies to
        /// </summary>
        public string EntityType { get; set; }
        
        /// <summary>
        /// Optional field name if trigger is field-specific
        /// </summary>
        public string FieldName { get; set; }
        
        /// <summary>
        /// Optional value to compare against for triggers like status changes
        /// </summary>
        public string ExpectedValue { get; set; }
        
        /// <summary>
        /// For scheduled triggers, specifies when to run
        /// </summary>
        public string ScheduleExpression { get; set; }
    }

    /// <summary>
    /// Data transfer object for WorkflowStep entity
    /// </summary>
    public class WorkflowStepDto : EntityDto
    {
        /// <summary>
        /// Order of execution within the workflow
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Name of the step
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what this step does
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Type of action to perform
        /// </summary>
        public AutomationActionType ActionType { get; set; }
        
        /// <summary>
        /// Configuration for the action in JSON format
        /// </summary>
        public string ActionConfiguration { get; set; }
        
        /// <summary>
        /// Optional delay before executing this step
        /// </summary>
        public TimeSpan? DelayBefore { get; set; }
        
        /// <summary>
        /// Whether this step is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }

    /// <summary>
    /// Data transfer object for WorkflowExecutionRecord entity
    /// </summary>
    public class WorkflowExecutionRecordDto : EntityDto<Guid>
    {
        /// <summary>
        /// Entity that triggered the workflow
        /// </summary>
        public Guid TriggeredByEntityId { get; set; }
        
        /// <summary>
        /// Type of entity that triggered the workflow
        /// </summary>
        public string TriggeredByEntityType { get; set; }
        
        /// <summary>
        /// When the execution started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// When the execution completed
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        
        /// <summary>
        /// Overall status of the execution
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Error message if execution failed
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Detailed execution log
        /// </summary>
        public string ExecutionLog { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating an automation workflow
    /// </summary>
    public class CreateAutomationWorkflowDto
    {
        /// <summary>
        /// Name of the workflow
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the workflow
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
        
        /// <summary>
        /// Trigger condition for the workflow
        /// </summary>
        [Required]
        public CreateAutomationTriggerConditionDto TriggerCondition { get; set; }
        
        /// <summary>
        /// Whether to activate the workflow immediately
        /// </summary>
        public bool Activate { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a trigger condition
    /// </summary>
    public class CreateAutomationTriggerConditionDto
    {
        /// <summary>
        /// The type of trigger
        /// </summary>
        [Required]
        public AutomationTriggerType TriggerType { get; set; }
        
        /// <summary>
        /// The entity type this trigger applies to
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }
        
        /// <summary>
        /// Optional field name if trigger is field-specific
        /// </summary>
        [StringLength(100)]
        public string FieldName { get; set; }
        
        /// <summary>
        /// Optional value to compare against for triggers like status changes
        /// </summary>
        [StringLength(100)]
        public string ExpectedValue { get; set; }
        
        /// <summary>
        /// For scheduled triggers, specifies when to run
        /// </summary>
        [StringLength(100)]
        public string ScheduleExpression { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating an automation workflow
    /// </summary>
    public class UpdateAutomationWorkflowDto
    {
        /// <summary>
        /// Name of the workflow
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the workflow
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a trigger condition
    /// </summary>
    public class UpdateAutomationTriggerConditionDto
    {
        /// <summary>
        /// The type of trigger
        /// </summary>
        public AutomationTriggerType? TriggerType { get; set; }
        
        /// <summary>
        /// The entity type this trigger applies to
        /// </summary>
        [StringLength(50)]
        public string EntityType { get; set; }
        
        /// <summary>
        /// Optional field name if trigger is field-specific
        /// </summary>
        [StringLength(100)]
        public string FieldName { get; set; }
        
        /// <summary>
        /// Optional value to compare against for triggers like status changes
        /// </summary>
        [StringLength(100)]
        public string ExpectedValue { get; set; }
        
        /// <summary>
        /// For scheduled triggers, specifies when to run
        /// </summary>
        [StringLength(100)]
        public string ScheduleExpression { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a workflow step
    /// </summary>
    public class CreateWorkflowStepDto
    {
        /// <summary>
        /// Name of the step
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what this step does
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
        
        /// <summary>
        /// Type of action to perform
        /// </summary>
        [Required]
        public AutomationActionType ActionType { get; set; }
        
        /// <summary>
        /// Configuration for the action in JSON format
        /// </summary>
        [Required]
        public string ActionConfiguration { get; set; }
        
        /// <summary>
        /// Optional delay before executing this step (in seconds)
        /// </summary>
        public int? DelayBeforeSeconds { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a workflow step
    /// </summary>
    public class UpdateWorkflowStepDto
    {
        /// <summary>
        /// Name of the step
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what this step does
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
        
        /// <summary>
        /// Type of action to perform
        /// </summary>
        public AutomationActionType? ActionType { get; set; }
        
        /// <summary>
        /// Configuration for the action in JSON format
        /// </summary>
        public string ActionConfiguration { get; set; }
        
        /// <summary>
        /// Optional delay before executing this step (in seconds)
        /// </summary>
        public int? DelayBeforeSeconds { get; set; }
    }

    /// <summary>
    /// Data transfer object for reordering workflow steps
    /// </summary>
    public class ReorderWorkflowStepsDto
    {
        /// <summary>
        /// List of step IDs in the new order
        /// </summary>
        [Required]
        public List<StepOrderItemDto> Steps { get; set; }
    }

    /// <summary>
    /// Data transfer object for a step order item
    /// </summary>
    public class StepOrderItemDto
    {
        /// <summary>
        /// Step ID
        /// </summary>
        [Required]
        public Guid StepId { get; set; }
        
        /// <summary>
        /// New order position
        /// </summary>
        [Required]
        public int NewOrder { get; set; }
    }

    /// <summary>
    /// Data transfer object for workflow list filtering and pagination
    /// </summary>
    public class GetAutomationWorkflowListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in workflow name and description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by active status
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Filter by trigger type
        /// </summary>
        public AutomationTriggerType? TriggerType { get; set; }
        
        /// <summary>
        /// Filter by entity type
        /// </summary>
        public string EntityType { get; set; }
    }

    /// <summary>
    /// Data transfer object for manually triggering a workflow
    /// </summary>
    public class TriggerWorkflowDto
    {
        /// <summary>
        /// Entity that is triggering the workflow
        /// </summary>
        [Required]
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Type of entity that is triggering the workflow
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }
    }
}
