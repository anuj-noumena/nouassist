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
    /// Task Checklist Completion - Runtime checklist progress tracking for a TaskInstance
    /// </summary>
    public class TaskChecklistCompletion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid ChecklistId { get; private set; } // Reference to TaskChecklist blueprint
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === CHECKLIST COMPLETION ITEMS ===
        private readonly List<TaskChecklistItem> _items = new();
        public IReadOnlyCollection<TaskChecklistItem> Items => _items.AsReadOnly();

        protected TaskChecklistCompletion() { }

        public TaskChecklistCompletion(
            Guid id,
            Guid taskInstanceId,
            Guid checklistId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            ChecklistId = checklistId;
            TenantId = tenantId;
            Version = 1;
        }

        // === INITIALIZATION ===
        public void InitializeFromBlueprint(List<Guid> checklistItemIds)
        {
            foreach (var itemId in checklistItemIds)
            {
                _items.Add(new TaskChecklistItem(TaskInstanceId, itemId));
            }
            Version++;
        }

        // === CHECKLIST MANAGEMENT ===
        public void CheckItem(Guid itemId, Guid? userId = null)
        {
            var item = _items.FirstOrDefault(i => i.ChecklistItemId == itemId);
            if (item == null)
            {
                throw new BusinessException("Tasks:ChecklistItemNotFound");
            }

            item.Check(userId);
            Version++;

            AddDistributedEvent(new TaskChecklistItemCheckedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ChecklistItemId = itemId,
                CheckedBy = userId,
                CheckedAt = DateTime.UtcNow,
                CompletionPercentage = GetCompletionPercentage()
            });
        }

        public void UncheckItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.ChecklistItemId == itemId);
            if (item == null)
            {
                throw new BusinessException("Tasks:ChecklistItemNotFound");
            }

            item.Uncheck();
            Version++;

            AddDistributedEvent(new TaskChecklistItemUncheckedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ChecklistItemId = itemId,
                UncheckedAt = DateTime.UtcNow,
                CompletionPercentage = GetCompletionPercentage()
            });
        }

        // === COMPUTED PROPERTIES ===
        public int TotalItems => _items.Count;
        public int CompletedItems => _items.Count(i => i.IsChecked);
        public double GetCompletionPercentage()
        {
            if (_items.Count == 0)
            {
                return 0;
            }
            return (double)CompletedItems / TotalItems * 100.0;
        }
        public bool IsFullyCompleted => _items.Count > 0 && _items.All(i => i.IsChecked);
    }
}
