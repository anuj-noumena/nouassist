using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// Task Acceptance Completion - Runtime acceptance criteria fulfillment tracking for a TaskInstance
    /// </summary>
    public class TaskAcceptanceCompletion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid AcceptanceCriteriaId { get; private set; } // Reference to TaskAcceptanceCriteria blueprint
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === ACCEPTANCE CRITERIA COMPLETIONS ===
        private readonly List<AcceptanceCriterionCompletion> _completions = new();
        public IReadOnlyCollection<AcceptanceCriterionCompletion> Completions => _completions.AsReadOnly();

        protected TaskAcceptanceCompletion() { }

        public TaskAcceptanceCompletion(
            Guid id,
            Guid taskInstanceId,
            Guid acceptanceCriteriaId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            AcceptanceCriteriaId = acceptanceCriteriaId;
            TenantId = tenantId;
            Version = 1;
        }

        // === INITIALIZATION ===
        public void InitializeFromBlueprint(List<Guid> criterionIds)
        {
            foreach (var criterionId in criterionIds)
            {
                _completions.Add(new AcceptanceCriterionCompletion(TaskInstanceId, criterionId));
            }
            Version++;
        }

        // === ACCEPTANCE CRITERIA MANAGEMENT ===
        public void MarkAsMet(Guid criterionId, Guid userId)
        {
            var completion = _completions.FirstOrDefault(c => c.CriterionId == criterionId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:AcceptanceCriteriaNotFound");
            }

            completion.MarkAsMet(userId);
            Version++;

            AddDistributedEvent(new TaskAcceptanceCriterionMetEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CriterionId = criterionId,
                MetBy = userId,
                MetAt = DateTime.UtcNow
            });
        }

        public void MarkAsUnmet(Guid criterionId)
        {
            var completion = _completions.FirstOrDefault(c => c.CriterionId == criterionId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:AcceptanceCriteriaNotFound");
            }

            completion.UnmarkAsMet();
            Version++;

            AddDistributedEvent(new TaskAcceptanceCriterionUnmetEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CriterionId = criterionId,
                UnmetAt = DateTime.UtcNow
            });
        }

        // === COMPUTED PROPERTIES ===
        public int TotalCriteria => _completions.Count;
        public int MetCriteria => _completions.Count(c => c.IsMet);
        public bool AllCriteriaMet => _completions.Count > 0 && _completions.All(c => c.IsMet);
    }
}
