using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Events;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// Task Blueprint - Defines the template for task instances
    /// </summary>
    public class Task : FullAuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public Guid? ProjectId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ExpectedResult { get; set; }
        
        public double DefaultImportance { get; private set; }
        public double DefaultUrgency { get; private set; }
        
        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? DefaultSprintId { get; set; }
        public Guid? TaskTypeId { get; set; }
        
        public int? EstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }
        
        public bool IsRecurring { get; private set; }
        public RecurrenceRule RecurrenceRule { get; private set; }
        
        public bool IsActive { get; private set; }
        public int Version { get; private set; }

        private readonly List<TaskActionStep> _actionSteps = new();
        public IReadOnlyCollection<TaskActionStep> ActionSteps => _actionSteps.AsReadOnly();

        private readonly List<AcceptanceCriterion> _acceptanceCriteria = new();
        public IReadOnlyCollection<AcceptanceCriterion> AcceptanceCriteria => 
            _acceptanceCriteria.AsReadOnly();

        protected Task() { /* ORM */ }

        public Task(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId = null,
            Guid? projectId = null,
            double defaultImportance = 0.5,
            double defaultUrgency = 0.5) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 500);
            TenantId = tenantId;
            ProjectId = projectId;
            
            SetImportanceUrgency(defaultImportance, defaultUrgency);
            
            IsActive = true;
            Version = 1;

            AddDistributedEvent(new TaskCreatedEto
            {
                TaskId = Id,
                TenantId = TenantId,
                Title = Title,
                OwnerId = OwnerId,
                CreatedAt = DateTime.UtcNow
            });
        }

        
        public void MoveToProject(Guid projectId, Guid workflowStatusId)
        {
            ProjectId = projectId;
        }

        // Action Steps Management
        public void AddActionStep(string title, string notes = null, bool isRequired = false, int seq = 0)
        {
            if (_actionSteps.Count >= 50) // Business rule: max 50 steps
            {
                throw new BusinessException("Tasks:MaxActionStepsExceeded");
            }

            var step = new TaskActionStep(Id, title, notes, isRequired, seq);
            _actionSteps.Add(step);
            Version++;
        }

        public void RemoveActionStep(Guid stepId)
        {
            var step = _actionSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                throw new BusinessException("Tasks:ActionStepNotFound");
            }

            _actionSteps.Remove(step);
            Version++;
        }

        public void ReorderActionSteps(List<Guid> orderedStepIds)
        {
            if (orderedStepIds.Count != _actionSteps.Count)
            {
                throw new BusinessException("Tasks:InvalidStepReorder");
            }

            for (int i = 0; i < orderedStepIds.Count; i++)
            {
                var step = _actionSteps.FirstOrDefault(s => s.Id == orderedStepIds[i]);
                if (step != null)
                {
                    step.UpdateSequence(i);
                }
            }
            Version++;
        }

        // Acceptance Criteria Management
        public void AddAcceptanceCriterion(
            string description,
            int? priority = null,
            string givenContext = null,
            string whenAction = null,
            string thenOutcome = null)
        {
            if (_acceptanceCriteria.Count >= 20) // Business rule
            {
                throw new BusinessException("Tasks:MaxAcceptanceCriteriaExceeded");
            }

            var criterion = new AcceptanceCriterion(
                Id, description, priority, givenContext, whenAction, thenOutcome);
            _acceptanceCriteria.Add(criterion);
            Version++;
        }

        public void RemoveAcceptanceCriterion(Guid criterionId)
        {
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion == null)
            {
                throw new BusinessException("Tasks:AcceptanceCriterionNotFound");
            }

            _acceptanceCriteria.Remove(criterion);
            Version++;
        }
        
        // Recurrence
        public void MakeRecurring(RecurrenceRule recurrenceRule)
        {
            RecurrenceRule = Check.NotNull(recurrenceRule, nameof(recurrenceRule));
            IsRecurring = true;
            Version++;
        }

        public void RemoveRecurrence()
        {
            RecurrenceRule = null;
            IsRecurring = false;
            Version++;
        }

        // State Management
        public void SetImportanceUrgency(double importance, double urgency)
        {
            var matrix = new ImportanceUrgencyMatrix(importance, urgency);
            DefaultImportance = matrix.Importance;
            DefaultUrgency = matrix.Urgency;
            Version++;
        }

        public void UpdateTitle(string newTitle)
        {
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle), maxLength: 500);
            Version++;
        }

        public void Deactivate()
        {
            IsActive = false;
            Version++;
        }

        public void Activate()
        {
            IsActive = true;
            Version++;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            IsActive = false;
            Version++;
        }
    }
}
