using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// Task Acceptance Criteria - Blueprint acceptance criteria for a Task
    /// </summary>
    public class TaskAcceptanceCriteria : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === ACCEPTANCE CRITERIA ===
        private readonly List<AcceptanceCriterion> _criteria = new();
        public IReadOnlyCollection<AcceptanceCriterion> Criteria => _criteria.AsReadOnly();

        protected TaskAcceptanceCriteria() { }

        public TaskAcceptanceCriteria(
            Guid id,
            Guid taskId,
            Guid? tenantId = null) : base(id)
        {
            TaskId = taskId;
            TenantId = tenantId;
            Version = 1;
        }

        // === ACCEPTANCE CRITERIA MANAGEMENT ===
        public void AddCriterion(string description, int? priority = null)
        {
            var criterion = new AcceptanceCriterion(TaskId, description, priority);
            _criteria.Add(criterion);
            Version++;

            AddDistributedEvent(new TaskAcceptanceCriterionAddedEto
            {
                TaskId = TaskId,
                TenantId = TenantId,
                CriterionId = criterion.Id,
                Description = description,
                Priority = priority,
                Version = Version,
                AddedAt = DateTime.UtcNow
            });
        }

        public void RemoveCriterion(Guid criterionId)
        {
            var criterion = _criteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion != null)
            {
                _criteria.Remove(criterion);
                Version++;

                AddDistributedEvent(new TaskAcceptanceCriterionRemovedEto
                {
                    TaskId = TaskId,
                    TenantId = TenantId,
                    CriterionId = criterionId,
                    Version = Version,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        // === COMPUTED PROPERTIES ===
        public bool HasCriteria => _criteria.Any();
        public int TotalCriteriaCount => _criteria.Count;
    }
}
