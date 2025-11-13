using System;
using NouAssist.SharedKernel.ValueObjects;
using NouAssist.WorkManagement.GTD.Enums;
using NouAssist.WorkManagement.GTD.Events;
using NouAssist.WorkManagement.GTD.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.GTD.Aggregates
{
    public class TicklerItem : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime TickleDate { get; private set; }
        public TimeSpan? TickleTime { get; private set; }
        public TicklerStatus Status { get; private set; }
        public DateTime? SurfacedAt { get; private set; }
        public DateTime? ActionedAt { get; private set; }
        public DateTime? DismissedAt { get; private set; }
        public DateTime? SnoozeUntil { get; private set; }
        public TicklerActionType? ActionType { get; private set; }
        public ImportanceScore Importance { get; private set; }
        public UrgencyScore Urgency { get; private set; }
        public bool IsRecurring { get; private set; }
        public RecurrenceRule? RecurrencePattern { get; private set; }
        public Guid? ContextId { get; set; }

        protected TicklerItem() { }

        public TicklerItem(
            Guid id, 
            Guid ownerId, 
            string title, 
            DateTime tickleDate,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            TickleDate = tickleDate;
            TenantId = tenantId;
            Status = TicklerStatus.Scheduled;
            Importance = new ImportanceScore(0.50);
            Urgency = new UrgencyScore(0.50);
            IsRecurring = false;
        }

        public void SetRecurrence(RecurrenceRule recurrencePattern)
        {
            IsRecurring = true;
            RecurrencePattern = recurrencePattern;
        }

        public void Surface()
        {
            if (Status == TicklerStatus.Surfaced)
            {
                return;
            }

            Status = TicklerStatus.Surfaced;
            SurfacedAt = DateTime.UtcNow;

            AddDistributedEvent(new TicklerSurfacedEto
            {
                TicklerId = Id,
                UserId = OwnerId,
                TenantId = TenantId,
                SurfacedAt = SurfacedAt.Value,
                Title = Title
            });
        }

        public void TakeAction(TicklerActionType actionType)
        {
            Status = TicklerStatus.Actioned;
            ActionType = actionType;
            ActionedAt = DateTime.UtcNow;
        }

        public void Dismiss()
        {
            Status = TicklerStatus.Dismissed;
            DismissedAt = DateTime.UtcNow;
        }

        public void Snooze(DateTime until)
        {
            Status = TicklerStatus.Snoozed;
            SnoozeUntil = until;
        }

        public void Reschedule(DateTime newDate)
        {
            TickleDate = newDate;
            Status = TicklerStatus.Scheduled;
            SnoozeUntil = null;
        }

        public bool IsScheduled => Status == TicklerStatus.Scheduled;
        public bool IsSurfaced => Status == TicklerStatus.Surfaced;
        public bool IsActioned => Status == TicklerStatus.Actioned;
        public bool IsDismissed => Status == TicklerStatus.Dismissed;
        public bool IsSnoozed => Status == TicklerStatus.Snoozed;
        public bool IsDue => DateTime.UtcNow >= TickleDate;

        public DateTime? GetNextOccurrence()
        {
            if (!IsRecurring || RecurrencePattern == null)
            {
                return null;
            }

            return RecurrencePattern.GetNextOccurrence(TickleDate);
        }
    }
}
