// File: TaskChecklistAggregate.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// Manages completable sub-items of a task, like checklists and acceptance criteria.
    /// </summary>
    public class TaskChecklistAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskInstanceId { get; private set; }

        private readonly List<TaskChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<TaskChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        private readonly List<AcceptanceCriterionCompletion> _criterionCompletions = new();
        public IReadOnlyCollection<AcceptanceCriterionCompletion> CriterionCompletions => _criterionCompletions.AsReadOnly();
        
        private readonly List<TaskActionStepCompletion> _actionStepsCompletions = new();
        public IReadOnlyCollection<TaskActionStepCompletion> ActionStepCompletions => _actionStepsCompletions.AsReadOnly();

        protected TaskChecklistAggregate() { }

        public TaskChecklistAggregate(Guid id, Guid taskInstanceId) : base(id)
        {
            TaskInstanceId = taskInstanceId;
        }

        // === Checklist Methods ===
        public void InitializeChecklist(List<Guid> checklistItemIds)
        {
            foreach (var itemId in checklistItemIds)
            {
                _checklistItems.Add(new TaskChecklistItem(Id, itemId));
            }
        }

        public void CheckItem(Guid itemId, Guid? userId = null)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == itemId) ?? throw new BusinessException("TaskManagement:ChecklistItemNotFound");
            item.Check(userId);
            AddDistributedEvent(new TaskChecklistItemCheckedEto { TaskChecklistAggregateId = Id, ChecklistItemId = itemId, CheckedBy = userId });
        }

        public void UncheckItem(Guid itemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == itemId) ?? throw new BusinessException("TaskManagement:ChecklistItemNotFound");
            item.Uncheck();
            AddDistributedEvent(new TaskChecklistItemUncheckedEto { TaskChecklistAggregateId = Id, ChecklistItemId = itemId });
        }
        
        // === Acceptance Criteria Methods ===
        public void InitializeAcceptanceCriteria(List<Guid> criteriaIds)
        {
            foreach (var itemId in criteriaIds)
            {
                _criterionCompletions.Add(new AcceptanceCriterionCompletion(Id, itemId));
            }
        }

        public void MarkCriterionAsMet(Guid itemId, Guid userId)
        {
            var item = _criterionCompletions.FirstOrDefault(i => i.CriterionId == itemId) ?? throw new BusinessException("TaskManagement:CriterionNotFound");
            item.MarkAsMet(userId);
            AddDistributedEvent(new TaskCriterionMetEto { TaskChecklistAggregateId = Id, CriterionId = itemId, MetBy = userId });
        }

        // === Action Step Methods ===
        public void InitializeActionSteps(List<Guid> actionStepIds)
        {
            foreach (var itemId in actionStepIds)
            {
                _actionStepsCompletions.Add(new TaskActionStepCompletion(Id, itemId));
            }
        }

        public void MarkActionStepComplete(Guid itemId, Guid userId)
        {
            var item = _actionStepsCompletions.FirstOrDefault(i => i.TaskActionStepId == itemId) ?? throw new BusinessException("TaskManagement:ActionStepNotFound");
            item.Complete(userId);
            AddDistributedEvent(new TaskActionStepCompletedEto { TaskChecklistAggregateId = Id, ActionStepId = itemId, CompletedBy = userId });
        }
    }
}