using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.Workflow.Aggregates
{
    /// <summary>
    /// Represents a Workflow aggregate root that defines a process composed of statuses
    /// and transitions between them. A workflow determines how a task or item progresses
    /// through various states in the system.
    /// </summary>
    public class Workflow : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the workflow (required).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional textual description for the workflow.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The identifier of the initial status from which this workflow begins.
        /// </summary>
        public Guid InitialStatusId { get; private set; }

        /// <summary>
        /// Indicates whether this workflow is the default one within its context (e.g., a project or scheme).
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// Indicates whether this workflow is currently active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Backing field containing all workflow statuses (states like "Open", "In Progress", "Done").
        /// </summary>
        private readonly List<WorkflowStatus> _statuses = new();

        /// <summary>
        /// Read-only collection of workflow statuses.
        /// </summary>
        public IReadOnlyCollection<WorkflowStatus> Statuses => _statuses.AsReadOnly();

        /// <summary>
        /// Backing field containing all allowed transitions between workflow statuses.
        /// </summary>
        private readonly List<WorkflowTransition> _transitions = new();

        /// <summary>
        /// Read-only collection of workflow transitions.
        /// </summary>
        public IReadOnlyCollection<WorkflowTransition> Transitions => _transitions.AsReadOnly();

        /// <summary>
        /// Custom properties associated with this workflow.
        /// Can store arbitrary key-value pairs (e.g., settings, metadata).
        /// </summary>
        public ExtraPropertyDictionary ExtraProperties { get; }


        /// <summary>
        /// Protected parameterless constructor required by EF Core and ABP.
        /// Initializes internal collections.
        /// </summary>
        protected Workflow()
        {
            ExtraProperties = new ExtraPropertyDictionary();
        }

        /// <summary>
        /// Constructs a new workflow instance with a name, initial status, and optional description.
        /// </summary>
        /// <param name="id">Unique identifier for the workflow.</param>
        /// <param name="name">Workflow name (required).</param>
        /// <param name="initialStatusId">The first status in this workflow.</param>
        /// <param name="description">Optional description text.</param>
        public Workflow(
            Guid id,
            string name,
            Guid initialStatusId,
            string description = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            InitialStatusId = initialStatusId;
            Description = description;

            // Workflows are active upon creation by default
            IsActive = true;

            ExtraProperties = new ExtraPropertyDictionary();
        }

        /// <summary>
        /// Adds a new status (state) to the workflow.
        /// Ensures that status names are unique within the workflow.
        /// </summary>
        /// <param name="status">Workflow status to add.</param>
        /// <exception cref="BusinessException">Thrown when a duplicate status name exists.</exception>
        public void AddStatus(WorkflowStatus status)
        {
            Check.NotNull(status, nameof(status));

            // Prevent duplicate statuses by name
            if (_statuses.Any(s => s.Name == status.Name))
            {
                throw new BusinessException("Workflow:DuplicateStatus",
                    $"Status '{status.Name}' already exists.");
            }

            _statuses.Add(status);
        }

        /// <summary>
        /// Adds a transition between two statuses (e.g., "Open" → "In Progress").
        /// Validates that both source and destination statuses exist in the workflow.
        /// </summary>
        /// <param name="transition">Transition entity to add.</param>
        /// <exception cref="BusinessException">Thrown if transition references invalid statuses.</exception>
        public void AddTransition(WorkflowTransition transition)
        {
            Check.NotNull(transition, nameof(transition));

            // Ensure both source and target statuses exist before adding transition
            if (!_statuses.Any(s => s.Id == transition.FromStatusId))
            {
                throw new BusinessException("Workflow:InvalidFromStatus");
            }

            if (!_statuses.Any(s => s.Id == transition.ToStatusId))
            {
                throw new BusinessException("Workflow:InvalidToStatus");
            }

            _transitions.Add(transition);
        }

        /// <summary>
        /// Retrieves a workflow status by its identifier.
        /// </summary>
        /// <param name="statusId">The ID of the status to fetch.</param>
        /// <returns>The matching <see cref="WorkflowStatus"/>.</returns>
        /// <exception cref="BusinessException">Thrown if no status with the given ID is found.</exception>
        public WorkflowStatus GetStatus(Guid statusId)
        {
            var status = _statuses.FirstOrDefault(s => s.Id == statusId);
            if (status == null)
            {
                throw new BusinessException("Workflow:StatusNotFound");
            }

            return status;
        }

        /// <summary>
        /// Returns all possible transitions available from the given current status.
        /// </summary>
        /// <param name="currentStatusId">The current status ID.</param>
        /// <returns>List of transitions that originate from the given status.</returns>
        public List<WorkflowTransition> GetAvailableTransitions(Guid currentStatusId)
        {
            return _transitions
                .Where(t => t.FromStatusId == currentStatusId)
                .ToList();
        }

        /// <summary>
        /// Marks this workflow as the default one.
        /// (Typically, only one workflow per project or scheme should be default.)
        /// </summary>
        public void SetAsDefault()
        {
            IsDefault = true;
        }

        /// <summary>
        /// Activates the workflow, allowing it to be used for tasks.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the workflow unless it is marked as default.
        /// Prevents accidental deactivation of a primary workflow.
        /// </summary>
        /// <exception cref="BusinessException">Thrown if trying to deactivate a default workflow.</exception>
        public void Deactivate()
        {
            if (IsDefault)
            {
                throw new BusinessException("Workflow:CannotDeactivateDefault");
            }

            IsActive = false;
        }
        
        public void ValidateBeforeActivation()
        {
            if (!_statuses.Any())
            {
                throw new BusinessException("Workflow:NoStatuses");
            }
    
            if (!_statuses.Any(s => s.IsInitial))
            {
                throw new BusinessException("Workflow:NoInitialStatus");
            }
    
            if (_statuses.Count(s => s.IsInitial) > 1)
            {
                throw new BusinessException("Workflow:MultipleInitialStatuses");
            }
    
            if (!_transitions.Any())
            {
                throw new BusinessException("Workflow:NoTransitions");
            }
    
            // Check for unreachable statuses
            var reachable = GetReachableStatuses();
            var unreachable = _statuses.Where(s => !reachable.Contains(s.Id)).ToList();
    
            if (unreachable.Any())
            {
                throw new BusinessException("Workflow:UnreachableStatuses",
                    $"{unreachable.Count} statuses are unreachable from initial status.");
            }
        }

        private HashSet<Guid> GetReachableStatuses()
        {
            var reachable = new HashSet<Guid>();
            var queue = new Queue<Guid>();
    
            queue.Enqueue(InitialStatusId);
            reachable.Add(InitialStatusId);
    
            while (queue.Any())
            {
                var current = queue.Dequeue();
                var outgoing = _transitions.Where(t => t.FromStatusId == current);
        
                foreach (var transition in outgoing)
                {
                    if (!reachable.Contains(transition.ToStatusId))
                    {
                        reachable.Add(transition.ToStatusId);
                        queue.Enqueue(transition.ToStatusId);
                    }
                }
            }
    
            return reachable;
        }
        
    }
}
