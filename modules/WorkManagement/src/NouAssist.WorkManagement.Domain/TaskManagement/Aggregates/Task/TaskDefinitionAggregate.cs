using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.Task
{
    /// <summary>
    /// Manages the structural definition of a task blueprint: checklists, criteria, and steps.
    /// </summary>
    public class TaskDefinitionAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskId { get; private set; }
        
        private readonly List<ChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<ChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        private readonly List<AcceptanceCriterion> _acceptanceCriteria = new();
        public IReadOnlyCollection<AcceptanceCriterion> AcceptanceCriteria => _acceptanceCriteria.AsReadOnly();

        private readonly List<TaskActionStep> _actionSteps = new();
        public IReadOnlyCollection<TaskActionStep> ActionSteps => _actionSteps.AsReadOnly();

        protected TaskDefinitionAggregate() { }

        public TaskDefinitionAggregate(Guid id, Guid TaskId) : base(id)
        {
            TaskId = TaskId;
        }

        // === CHECKLIST MANAGEMENT ===
        public void AddChecklistItem(string title, bool isRequired = false)
        {
            var maxOrder = _checklistItems.Any() ? _checklistItems.Max(i => i.OrderIndex) : -1;
            var item = new ChecklistItem(Guid.NewGuid(), Id, title, maxOrder + 1, isRequired);
            _checklistItems.Add(item);
            AddDistributedEvent(new TaskDefinitionItemAddedEto { Type = "Checklist", TaskDefinitionId = Id, ItemId = item.Id });
        }

        public void RemoveChecklistItem(Guid itemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _checklistItems.Remove(item);
                ReorderChecklistItems();
                AddDistributedEvent(new TaskDefinitionItemRemovedEto { Type = "Checklist", TaskDefinitionId = Id, ItemId = itemId });
            }
        }
        
        // === ACTION STEPS MANAGEMENT ===
        public void AddActionStep(string title, string? notes = null, bool isRequired = false)
        {
            var maxOrder = _actionSteps.Any() ? _actionSteps.Max(i => i.Sequence) : -1;
            var item = new TaskActionStep(Guid.NewGuid(), Id, title, notes, isRequired, maxOrder + 1);
            _actionSteps.Add(item);
            AddDistributedEvent(new TaskDefinitionItemAddedEto { Type = "ActionStep", TaskDefinitionId = Id, ItemId = item.Id });
        }

        public void RemoveActionStep(Guid itemId)
        {
            var item = _actionSteps.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _actionSteps.Remove(item);
                ReorderActionSteps();
                AddDistributedEvent(new TaskDefinitionItemRemovedEto { Type = "ActionStep", TaskDefinitionId = Id, ItemId = itemId });
            }
        }

        // === ACCEPTANCE CRITERIA MANAGEMENT ===
        public void AddAcceptanceCriterion(string description, int? priority = null)
        {
            var criterion = new AcceptanceCriterion(Id, description, priority);
            _acceptanceCriteria.Add(criterion);
            AddDistributedEvent(new TaskDefinitionItemAddedEto { Type = "Criterion", TaskDefinitionId = Id, ItemId = criterion.Id });
        }
        
        public void RemoveAcceptanceCriterion(Guid criterionId)
        {
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion != null)
            {
                _acceptanceCriteria.Remove(criterion);
                AddDistributedEvent(new TaskDefinitionItemRemovedEto { Type = "Criterion", TaskDefinitionId = Id, ItemId = criterionId });
            }
        }
        
        // === PRIVATE REORDERING HELPERS ===
        private void ReorderChecklistItems()
        {
            var items = _checklistItems.OrderBy(i => i.OrderIndex).ToList();
            for (int i = 0; i < items.Count; i++) items[i].UpdateOrder(i);
        }

        private void ReorderActionSteps()
        {
            var items = _actionSteps.OrderBy(i => i.Sequence).ToList();
            for (int i = 0; i < items.Count; i++) items[i].UpdateSequence(i);
        }
    }
}