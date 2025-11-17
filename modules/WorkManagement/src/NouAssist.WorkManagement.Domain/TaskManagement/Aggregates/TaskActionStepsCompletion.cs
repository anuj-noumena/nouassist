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
    /// Task Action Steps Completion - Runtime action steps progress tracking for a TaskInstance
    /// </summary>
    public class TaskActionStepsCompletion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === ACTION STEP COMPLETIONS ===
        private readonly List<TaskActionStepCompletion> _completions = new();
        public IReadOnlyCollection<TaskActionStepCompletion> Completions => _completions.AsReadOnly();

        protected TaskActionStepsCompletion() { }

        public TaskActionStepsCompletion(
            Guid id,
            Guid taskInstanceId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;
            Version = 1;
        }

        // === INITIALIZATION ===
        public void InitializeFromBlueprint(List<Guid> actionStepIds)
        {
            foreach (var stepId in actionStepIds)
            {
                _completions.Add(new TaskActionStepCompletion(TaskInstanceId, stepId));
            }
            Version++;
        }

        // === ACTION STEP MANAGEMENT ===
        public void MarkComplete(Guid stepId, Guid userId)
        {
            var completion = _completions.FirstOrDefault(c => c.TaskActionStepId == stepId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:ActionStepNotFound");
            }

            completion.Complete(userId);
            Version++;

            AddDistributedEvent(new TaskActionStepCompletedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ActionStepId = stepId,
                CompletedBy = userId,
                CompletedAt = DateTime.UtcNow
            });
        }

        public void MarkUncomplete(Guid stepId)
        {
            var completion = _completions.FirstOrDefault(c => c.TaskActionStepId == stepId);
            if (completion == null)
            {
                throw new BusinessException("Tasks:ActionStepNotFound");
            }

            completion.Uncomplete();
            Version++;

            AddDistributedEvent(new TaskActionStepUncompletedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ActionStepId = stepId,
                UncompletedAt = DateTime.UtcNow
            });
        }

        // === COMPUTED PROPERTIES ===
        public int TotalSteps => _completions.Count;
        public int CompletedSteps => _completions.Count(c => c.IsCompleted);
        public double GetCompletionPercentage()
        {
            if (_completions.Count == 0)
            {
                return 0;
            }
            return (double)CompletedSteps / TotalSteps * 100.0;
        }
        public bool AllStepsCompleted => _completions.Count > 0 && _completions.All(c => c.IsCompleted);
    }
}
