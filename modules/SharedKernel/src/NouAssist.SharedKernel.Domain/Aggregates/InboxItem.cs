using System;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Aggregates
{
    public class InboxItem : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string InboxContent { get; private set; }
        public InboxItemType ItemType { get; set; }
        public InboxItemSource Source { get; private set; }
        public bool Processed { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? ProcessedToEntityType { get; private set; }
        public Guid? ProcessedToEntityId { get; private set; }
        public Guid? ProcessedBy { get; private set; }
        public string? Notes { get; set; }

        protected InboxItem() { }

        public InboxItem(
            Guid id, 
            Guid ownerId, 
            string content, 
            InboxItemType itemType = InboxItemType.Other,
            InboxItemSource source = InboxItemSource.Manual,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            InboxContent = Check.NotNullOrWhiteSpace(content, nameof(content), maxLength: 5000);
            ItemType = itemType;
            Source = source;
            TenantId = tenantId;
            Processed = false;
        }

        public void UpdateContent(string content)
        {
            if (Processed)
            {
                throw new BusinessException("Core:CannotModifyProcessedItem", 
                    "Cannot modify an already processed inbox item.");
            }
            InboxContent = Check.NotNullOrWhiteSpace(content, nameof(content), maxLength: 5000);
        }

        public void MarkAsProcessed(Guid processedBy, string? entityType = null, Guid? entityId = null)
        {
            if (Processed)
            {
                throw new BusinessException("Core:ItemAlreadyProcessed", 
                    "Inbox item is already processed.");
            }

            Processed = true;
            ProcessedAt = DateTime.UtcNow;
            ProcessedBy = processedBy;
            ProcessedToEntityType = entityType;
            ProcessedToEntityId = entityId;

            AddDistributedEvent(new InboxItemProcessedEto
            {
                InboxItemId = Id,
                UserId = OwnerId,
                TenantId = TenantId,
                ProcessedAt = ProcessedAt.Value,
                ProcessedToEntityType = entityType,
                ProcessedToEntityId = entityId
            });
        }

        public TimeSpan GetAge() => DateTime.UtcNow - CreationTime;
        public int GetAgeInDays() => GetAge().Days;
        public bool IsStale() => GetAgeInDays() > 7;
        public bool HasBeenProcessed => Processed;
        public bool IsPending => !Processed;
    }
}
