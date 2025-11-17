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
    /// Task Checklist - Blueprint checklist items for a Task
    /// </summary>
    public class TaskChecklist : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === CHECKLIST ITEMS ===
        private readonly List<ChecklistItem> _items = new();
        public IReadOnlyCollection<ChecklistItem> Items => _items.AsReadOnly();

        protected TaskChecklist() { }

        public TaskChecklist(
            Guid id,
            Guid taskId,
            Guid? tenantId = null) : base(id)
        {
            TaskId = taskId;
            TenantId = tenantId;
            Version = 1;
        }

        // === CHECKLIST MANAGEMENT ===
        public void AddItem(string title, bool isRequired = false)
        {
            var maxOrder = _items.Any()
                ? _items.Max(i => i.OrderIndex)
                : -1;

            var item = new ChecklistItem(
                Guid.NewGuid(),
                TaskId,
                title,
                maxOrder + 1,
                isRequired);

            _items.Add(item);
            Version++;

            AddDistributedEvent(new TaskChecklistItemAddedEto
            {
                TaskId = TaskId,
                TenantId = TenantId,
                ChecklistItemId = item.Id,
                ItemTitle = title,
                IsRequired = isRequired,
                OrderIndex = maxOrder + 1,
                Version = Version,
                AddedAt = DateTime.UtcNow
            });
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _items.Remove(item);
                ReorderItems();
                Version++;

                AddDistributedEvent(new TaskChecklistItemRemovedEto
                {
                    TaskId = TaskId,
                    TenantId = TenantId,
                    ChecklistItemId = itemId,
                    Version = Version,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        public void ReorderItems(List<Guid> orderedIds)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = _items.FirstOrDefault(ci => ci.Id == orderedIds[i]);
                item?.UpdateOrder(i);
            }
            Version++;
        }

        private void ReorderItems()
        {
            var items = _items.OrderBy(i => i.OrderIndex).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].UpdateOrder(i);
            }
        }

        // === COMPUTED PROPERTIES ===
        public bool HasItems => _items.Any();
        public int TotalItemCount => _items.Count;
        public int RequiredItemCount => _items.Count(i => i.IsRequired);
    }
}
