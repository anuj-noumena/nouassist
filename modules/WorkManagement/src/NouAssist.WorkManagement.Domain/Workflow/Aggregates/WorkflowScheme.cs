using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Workflow.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Workflow.Aggregates
{
    /// <summary>
    /// Represents a workflow scheme that defines mappings between task types and workflows.
    /// A WorkflowScheme is tied to a specific project and determines which workflow to use
    /// for each type of task within that project.
    /// </summary>
    public class WorkflowScheme : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the workflow scheme (required).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional description providing additional context.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Reference to the project this workflow scheme belongs to.
        /// </summary>
        public Guid ProjectId { get; private set; }

        /// <summary>
        /// Default workflow ID to be used when no specific mapping is found for a task type.
        /// </summary>
        public Guid? DefaultWorkflowId { get; private set; }

        /// <summary>
        /// Indicates if this workflow scheme is currently active and in use.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Indicates whether this workflow scheme is still in draft mode (not yet published).
        /// </summary>
        public bool IsDraft { get; private set; }

        /// <summary>
        /// Determines if quick task delegation is allowed under this workflow scheme.
        /// </summary>
        public bool AllowQuickDelegation { get; private set; } = true;

        /// <summary>
        /// Determines if delegation for integrated workflows requires approval.
        /// </summary>
        public bool RequireApprovalForIntegrated { get; private set; } = false;

        /// <summary>
        /// Backing field for the collection of task type → workflow mappings.
        /// </summary>
        private readonly List<WorkflowSchemeMapping> _mappings = new();

        /// <summary>
        /// Exposes mappings as a read-only collection.
        /// </summary>
        public IReadOnlyCollection<WorkflowSchemeMapping> Mappings => 
            _mappings.AsReadOnly();

        /// <summary>
        /// Protected constructor required by EF Core and ABP for ORM purposes.
        /// </summary>
        protected WorkflowScheme() { }

        /// <summary>
        /// Creates a new workflow scheme instance.
        /// </summary>
        /// <param name="id">Unique identifier of the workflow scheme.</param>
        /// <param name="name">Name of the workflow scheme.</param>
        /// <param name="projectId">Associated project identifier.</param>
        /// <param name="description">Optional description.</param>
        /// <param name="defaultWorkflowId">Optional default workflow.</param>
        public WorkflowScheme(
            Guid id,
            string name,
            Guid projectId,
            string description = null,
            Guid? defaultWorkflowId = null) : base(id)
        {
            // Validate and assign core properties
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ProjectId = projectId;
            Description = description;
            DefaultWorkflowId = defaultWorkflowId;

            // Workflow schemes start in draft mode and are inactive by default
            IsDraft = true;
            IsActive = false;
        }

        /// <summary>
        /// Adds a new mapping between a task type and a workflow.
        /// Ensures that no duplicate task type mapping exists.
        /// </summary>
        /// <param name="mapping">Mapping object to add.</param>
        /// <exception cref="BusinessException">Thrown when a duplicate task type mapping is detected.</exception>
        public void AddMapping(WorkflowSchemeMapping mapping)
        {
            Check.NotNull(mapping, nameof(mapping));

            // Prevent duplicate task type mappings
            if (_mappings.Any(m => m.TaskTypeId == mapping.TaskTypeId))
            {
                throw new BusinessException("Workflow:DuplicateTaskTypeMapping");
            }

            _mappings.Add(mapping);
        }

        /// <summary>
        /// Removes an existing mapping for a given task type.
        /// </summary>
        /// <param name="taskTypeId">The task type ID to remove the mapping for.</param>
        public void RemoveMapping(Guid taskTypeId)
        {
            var mapping = _mappings.FirstOrDefault(m => m.TaskTypeId == taskTypeId);
            if (mapping != null)
            {
                _mappings.Remove(mapping);
            }
        }

        /// <summary>
        /// Retrieves the workflow ID associated with a given task type.
        /// Falls back to the default workflow if no mapping is found.
        /// </summary>
        /// <param name="taskTypeId">Task type ID.</param>
        /// <returns>Workflow ID for the given task type.</returns>
        /// <exception cref="BusinessException">
        /// Thrown if no matching workflow or default workflow exists.
        /// </exception>
        public Guid GetWorkflowForTaskType(Guid taskTypeId)
        {
            // Look for an active mapping for the given task type
            var mapping = _mappings.FirstOrDefault(m => 
                m.TaskTypeId == taskTypeId && m.IsActive);

            if (mapping != null)
            {
                return mapping.WorkflowId;
            }

            // Use default workflow if present
            if (DefaultWorkflowId.HasValue)
            {
                return DefaultWorkflowId.Value;
            }

            // Otherwise, no workflow is defined for this type
            throw new BusinessException("Workflow:NoWorkflowForTaskType");
        }

        /// <summary>
        /// Sets or updates the default workflow to be used when no specific mapping exists.
        /// </summary>
        public void SetDefaultWorkflow(Guid workflowId)
        {
            DefaultWorkflowId = workflowId;
        }

        /// <summary>
        /// Publishes the workflow scheme — marking it as no longer a draft.
        /// A scheme must have either a default workflow or at least one mapping to be published.
        /// </summary>
        /// <exception cref="BusinessException">Thrown when trying to publish an empty scheme.</exception>
        public void Publish()
        {
            if (!DefaultWorkflowId.HasValue && !_mappings.Any())
            {
                throw new BusinessException("Workflow:CannotPublishEmptyScheme");
            }

            IsDraft = false;
        }

        /// <summary>
        /// Activates the workflow scheme.
        /// A draft scheme cannot be activated.
        /// </summary>
        /// <exception cref="BusinessException">Thrown when attempting to activate a draft scheme.</exception>
        public void Activate()
        {
            if (IsDraft)
            {
                throw new BusinessException("Workflow:CannotActivateDraft");
            }

            IsActive = true;
        }

        /// <summary>
        /// Deactivates the workflow scheme.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Updates delegation-related settings for this workflow scheme.
        /// </summary>
        /// <param name="allowQuick">Whether quick delegation is allowed.</param>
        /// <param name="requireApprovalForIntegrated">
        /// Whether delegation in integrated workflows requires approval.
        /// </param>
        public void SetDelegationMode(bool allowQuick, bool requireApprovalForIntegrated = false)
        {
            AllowQuickDelegation = allowQuick;
            RequireApprovalForIntegrated = requireApprovalForIntegrated;
        }
    }
}
