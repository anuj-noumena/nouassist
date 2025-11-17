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
    /// TaskInstanceChecklist - Manages checklist items for a specific task instance
    /// Separated from TaskInstance aggregate for better lifecycle management and scalability
    /// </summary>
    public class TaskInstanceChecklist : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid? TenantId { get; private set; }

        // === CHILD COLLECTIONS ===
        private readonly List<TaskChecklistItem> _checklistItems = new();
        public IReadOnlyCollection<TaskChecklistItem> ChecklistItems => _checklistItems.AsReadOnly();

        protected TaskInstanceChecklist() { }

        public TaskInstanceChecklist(
            Guid id,
            Guid taskInstanceId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;

            AddDistributedEvent(new TaskInstanceChecklistCreatedEto
            {
                TaskInstanceChecklistId = Id,
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // === CHECKLIST MANAGEMENT ===

        /// <summary>
        /// Initializes checklist items from the task template
        /// </summary>
        public void InitializeFromTemplate(List<Guid> checklistItemIds)
        {
            foreach (var itemId in checklistItemIds)
            {
                if (!_checklistItems.Any(ci => ci.ChecklistItemId == itemId))
                {
                    _checklistItems.Add(new TaskChecklistItem(TaskInstanceId, itemId));
                }
            }
        }

        /// <summary>
        /// Adds a new checklist item to this instance
        /// </summary>
        public void AddChecklistItem(Guid checklistItemId)
        {
            if (_checklistItems.Any(ci => ci.ChecklistItemId == checklistItemId))
            {
                throw new BusinessException("TaskInstanceChecklist:ItemAlreadyExists");
            }

            _checklistItems.Add(new TaskChecklistItem(TaskInstanceId, checklistItemId));
        }

        /// <summary>
        /// Checks a checklist item
        /// </summary>
        public void CheckItem(Guid checklistItemId, Guid? userId = null)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == checklistItemId);
            if (item == null)
            {
                throw new BusinessException("TaskInstanceChecklist:ItemNotFound");
            }

            item.Check(userId);

            AddDistributedEvent(new TaskChecklistItemCheckedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ChecklistItemId = checklistItemId,
                CheckedBy = userId,
                CheckedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Unchecks a checklist item
        /// </summary>
        public void UncheckItem(Guid checklistItemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == checklistItemId);
            if (item == null)
            {
                throw new BusinessException("TaskInstanceChecklist:ItemNotFound");
            }

            item.Uncheck();

            AddDistributedEvent(new TaskChecklistItemUncheckedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ChecklistItemId = checklistItemId,
                UncheckedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes a checklist item
        /// </summary>
        public void RemoveChecklistItem(Guid checklistItemId)
        {
            var item = _checklistItems.FirstOrDefault(i => i.ChecklistItemId == checklistItemId);
            if (item != null)
            {
                _checklistItems.Remove(item);
            }
        }

        /// <summary>
        /// Checks all items at once
        /// </summary>
        public void CheckAllItems(Guid? userId = null)
        {
            foreach (var item in _checklistItems.Where(i => !i.IsChecked))
            {
                item.Check(userId);
            }
        }

        /// <summary>
        /// Unchecks all items at once
        /// </summary>
        public void UncheckAllItems()
        {
            foreach (var item in _checklistItems.Where(i => i.IsChecked))
            {
                item.Uncheck();
            }
        }

        // === COMPUTED PROPERTIES ===
        public int TotalItemCount => _checklistItems.Count;
        public int CheckedItemCount => _checklistItems.Count(i => i.IsChecked);
        public int UncheckedItemCount => _checklistItems.Count(i => !i.IsChecked);
        public double CompletionPercentage => TotalItemCount == 0
            ? 0
            : (double)CheckedItemCount / TotalItemCount * 100.0;
        public bool IsFullyCompleted => TotalItemCount > 0 && CheckedItemCount == TotalItemCount;
        public bool HasItems => _checklistItems.Any();
    }
}
