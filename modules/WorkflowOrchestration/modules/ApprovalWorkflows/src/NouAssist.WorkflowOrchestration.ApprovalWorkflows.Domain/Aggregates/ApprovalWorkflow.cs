using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Entities;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using NouAssist.SharedKernel.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates
{
    /// <summary>
    /// Represents an approval workflow that defines a set of sequential or parallel approval steps
    /// and governs how approval processes are executed within the system.
    /// </summary>
    public class ApprovalWorkflow : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The unique name of the approval workflow.
        /// Used to identify the workflow type in business processes.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional description providing context or details about this workflow.
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Specifies the approval type 
        /// </summary>
        public ApprovalWorkflowType WorkflowType { get; private set; }
        
        /// <summary>
        /// Defines the type of workflow (e.g., Sequential, Parallel, or Custom).
        /// </summary>
        public ApprovalStrategy Strategy { get; private set; }

        /// <summary>
        /// Indicates whether the workflow is currently active and can be used for approvals.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Determines whether approvers are allowed to reassign approval tasks to others.
        /// </summary>
        public bool AllowReassign { get; private set; }

        /// <summary>
        /// Determines whether approvers are allowed to delegate their approval tasks temporarily.
        /// </summary>
        public bool AllowDelegate { get; private set; }

        /// <summary>
        /// Number of days before the workflow times out (if applicable).
        /// Used to enforce SLA or escalation rules.
        /// </summary>
        public int? TimeoutDays { get; private set; }

        /// <summary>
        /// Defines the action that should be taken automatically if the workflow times out 
        /// (e.g., auto-approve, auto-reject, escalate, etc.).
        /// </summary>
        public string TimeoutAction { get; private set; }

        /// <summary>
        /// Stores additional custom data defined at runtime, such as dynamic field values 
        /// or configuration options not part of the core schema.
        /// </summary>
        public CustomFieldValues CustomFields { get; private set; }
        
        /// <summary>
        /// Arbitrary metadata for system or integration use, 
        /// such as workflow version, created by external system, or audit flags.
        /// </summary>
        public Dictionary<string, object> Metadata { get; private set; }

        /// <summary>
        /// Identifier of the tenant to which this workflow belongs (used for multi-tenancy).
        /// </summary>
        public Guid? TenantId { get; private set; }

        /// <summary>
        /// The internal list of steps that make up this workflow.
        /// </summary>
        private readonly List<ApprovalStep> _steps = new();

        /// <summary>
        /// Read-only collection of all approval steps in this workflow, 
        /// ordered by their defined sequence.
        /// </summary>
        public IReadOnlyCollection<ApprovalStep> Steps => _steps.AsReadOnly();

        /// <summary>
        /// Protected constructor required by the ORM (e.g., EF Core).
        /// </summary>
        protected ApprovalWorkflow() { }

        /// <summary>
        /// Initializes a new approval workflow with required fields.
        /// </summary>
        public ApprovalWorkflow(
            Guid id,
            string name,
            ApprovalWorkflowType workflowType,
            ApprovalStrategy strategy,
            Guid? tenantId,
            string description = null
        ) : base(id)
        {
            SetName(name);
            WorkflowType = workflowType;
            Strategy = strategy;
            Description = description;
            TenantId = tenantId;
            IsActive = true;
            AllowReassign = false;
            AllowDelegate = false;
            CustomFields = CustomFieldValues.Create();
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// Validates and sets the workflow name.
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
        }

        /// <summary>
        /// Validates and sets the workflow description.
        /// </summary>
        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 2000)
            {
                throw new BusinessException("Approval:DescriptionTooLong")
                    .WithData("MaxLength", 2000);
            }
            Description = description;
        }

        /// <summary>
        /// Activates the workflow, making it available for use in approval processes.
        /// A workflow must contain at least one step before activation.
        /// </summary>
        public void Activate()
        {
            if (!_steps.Any())
            {
                throw new BusinessException("Approval:CannotActivateWithoutSteps")
                    .WithData("WorkflowId", Id);
            }
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the workflow, preventing it from being used in new approval processes.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Enables reassignment functionality for this workflow.
        /// </summary>
        public void EnableReassignment()
        {
            AllowReassign = true;
        }

        /// <summary>
        /// Enables task delegation functionality for this workflow.
        /// </summary>
        public void EnableDelegation()
        {
            AllowDelegate = true;
        }

        /// <summary>
        /// Sets a timeout duration and corresponding timeout action for this workflow.
        /// </summary>
        public void SetTimeout(int days, string action)
        {
            if (days <= 0)
            {
                throw new BusinessException("Approval:InvalidTimeoutDays");
            }
            TimeoutDays = days;
            TimeoutAction = action;
        }

        /// <summary>
        /// Adds a new approval step to the workflow.
        /// </summary>
        public ApprovalStep AddStep(
            string name,
            int orderIndex,
            ApprovalRequirement requirement,
            string description = null
        )
        {
            if (!IsActive)
            {
                throw new BusinessException("Approval:CannotModifyActiveWorkflow");
            }

            if (_steps.Any(s => s.OrderIndex == orderIndex))
            {
                throw new BusinessException("Approval:DuplicateStepOrder")
                    .WithData("OrderIndex", orderIndex);
            }

            var step = new ApprovalStep(
                Guid.NewGuid(),
                Id,
                name,
                orderIndex,
                requirement,
                description
            );

            _steps.Add(step);
            return step;
        }

        /// <summary>
        /// Removes an approval step by its unique identifier.
        /// </summary>
        public void RemoveStep(Guid stepId)
        {
            if (!IsActive)
            {
                throw new BusinessException("Approval:CannotModifyActiveWorkflow");
            }

            var step = _steps.FirstOrDefault(s => s.Id == stepId);
            if (step != null)
            {
                _steps.Remove(step);
                ReorderSteps();
            }
        }

        /// <summary>
        /// Ensures step ordering remains continuous and sequential after modifications.
        /// </summary>
        public void ReorderSteps()
        {
            var orderedSteps = _steps.OrderBy(s => s.OrderIndex).ToList();
            for (int i = 0; i < orderedSteps.Count; i++)
            {
                orderedSteps[i].SetOrderIndex(i + 1);
            }
        }

        /// <summary>
        /// Retrieves a specific step by its unique identifier.
        /// </summary>
        public ApprovalStep GetStep(Guid stepId)
        {
            return _steps.FirstOrDefault(s => s.Id == stepId)
                ?? throw new BusinessException("Approval:StepNotFound")
                    .WithData("StepId", stepId);
        }

        /// <summary>
        /// Gets the next step in the workflow sequence after the specified step.
        /// </summary>
        public ApprovalStep GetNextStep(Guid currentStepId)
        {
            var currentStep = GetStep(currentStepId);
            return _steps
                .Where(s => s.OrderIndex > currentStep.OrderIndex)
                .OrderBy(s => s.OrderIndex)
                .FirstOrDefault();
        }

        /// <summary>
        /// Determines whether a given step is the final step in the workflow.
        /// </summary>
        public bool IsLastStep(Guid stepId)
        {
            var step = GetStep(stepId);
            return step.OrderIndex == _steps.Max(s => s.OrderIndex);
        }

        /// <summary>
        /// Returns the total number of steps defined in this workflow.
        /// </summary>
        public int GetTotalSteps() => _steps.Count;

        /// <summary>
        /// Sets or updates a custom field value in the workflow’s dynamic data.
        /// </summary>
        public void SetCustomFieldValue(string key, object value)
        {
            CustomFields = CustomFields.SetValue(key, value);
        }
        
        /// <summary>
        /// Retrieves a custom field value by key, if available.
        /// </summary>
        public object GetCustomFieldValue(string key)
        {
            return CustomFields.GetValue(key);
        }
    }
}
