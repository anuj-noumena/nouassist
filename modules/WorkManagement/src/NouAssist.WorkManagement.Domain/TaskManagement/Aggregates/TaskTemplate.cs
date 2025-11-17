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
    /// TaskTemplate - Manages template specifications (checklist definitions and acceptance criteria) for Tasks
    /// Separated from Task aggregate to provide better lifecycle management and consistency boundaries
    /// </summary>
    public class TaskTemplate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskId { get; private set; }
        public Guid? TenantId { get; private set; }

        // === CHECKLIST ===
        private readonly List<ChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<ChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        // === ACCEPTANCE CRITERIA ===
        private readonly List<AcceptanceCriterion> _acceptanceCriteria = new();
        public IReadOnlyCollection<AcceptanceCriterion> AcceptanceCriteria =>
            _acceptanceCriteria.AsReadOnly();

        protected TaskTemplate() { }

        public TaskTemplate(
            Guid id,
            Guid taskId,
            Guid? tenantId = null) : base(id)
        {
            TaskId = taskId;
            TenantId = tenantId;

            AddDistributedEvent(new TaskTemplateCreatedEto
            {
                TaskTemplateId = Id,
                TaskId = TaskId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // === CHECKLIST MANAGEMENT ===
        public void AddChecklistItem(string title, bool isRequired = false)
        {
            var maxOrder = _checklistItems.Any()
                ? _checklistItems.Max(i => i.OrderIndex)
                : -1;

            var item = new ChecklistItem(
                Guid.NewGuid(),
                TaskId,
                title,
                maxOrder + 1,
                isRequired);

            _checklistItems.Add(item);

            AddDistributedEvent(new TaskChecklistItemAddedEto
            {
                TaskId = TaskId,
                TenantId = TenantId,
                ChecklistItemId = item.Id,
                ItemTitle = title,
                IsRequired = isRequired,
                OrderIndex = maxOrder + 1,
                Version = 1,
                AddedAt = DateTime.UtcNow
            });
        }

        public void RemoveChecklistItem(Guid itemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _checklistItems.Remove(item);
                ReorderChecklistItems();

                AddDistributedEvent(new TaskChecklistItemRemovedEto
                {
                    TaskId = TaskId,
                    TenantId = TenantId,
                    ChecklistItemId = itemId,
                    Version = 1,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        public void ReorderChecklistItems(List<Guid> orderedIds)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = _checklistItems.FirstOrDefault(ci => ci.Id == orderedIds[i]);
                item?.UpdateOrder(i);
            }
        }

        private void ReorderChecklistItems()
        {
            var items = _checklistItems.OrderBy(i => i.OrderIndex).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].UpdateOrder(i);
            }
        }

        // === ACCEPTANCE CRITERIA ===
        public void AddAcceptanceCriterion(string description, int? priority = null)
        {
            var criterion = new AcceptanceCriterion(TaskId, description, priority);
            _acceptanceCriteria.Add(criterion);
        }

        public void RemoveAcceptanceCriterion(Guid criterionId)
        {
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion != null)
            {
                _acceptanceCriteria.Remove(criterion);
            }
        }

        public void UpdateAcceptanceCriterion(
            Guid criterionId,
            string description,
            string givenContext,
            string whenAction,
            string thenOutcome,
            int? priority = null)
        {
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion == null)
            {
                throw new BusinessException("TaskTemplate:AcceptanceCriterionNotFound");
            }

            criterion.Update(description, givenContext, whenAction, thenOutcome, priority);
        }

        // === COMPUTED ===
        public bool HasChecklist => _checklistItems.Any();
        public bool HasAcceptanceCriteria => _acceptanceCriteria.Any();
        public int RequiredChecklistItemCount => _checklistItems.Count(i => i.IsRequired);
        public int TotalChecklistItemCount => _checklistItems.Count;
        public int AcceptanceCriteriaCount => _acceptanceCriteria.Count;
    }
}
