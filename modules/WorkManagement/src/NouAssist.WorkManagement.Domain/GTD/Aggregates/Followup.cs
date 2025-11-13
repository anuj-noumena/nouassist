using System;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.GTD.Aggregates
{
    public class Followup : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TaskInstanceId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public DateTime FollowupAt { get; private set; }
        public string? Note { get; set; }
        public FollowupStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        protected Followup() { }

        public Followup(
            Guid id,
            Guid ownerId,
            DateTime followupAt,
            Guid? taskInstanceId = null,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            FollowupAt = followupAt;
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;
            Status = FollowupStatus.Scheduled;
        }

        public void MarkAsDone()
        {
            if (Status == FollowupStatus.Done)
            {
                return;
            }

            Status = FollowupStatus.Done;
            CompletedAt = DateTime.UtcNow;
        }

        public void Dismiss()
        {
            Status = FollowupStatus.Dismissed;
        }

        public void Reschedule(DateTime newDate)
        {
            FollowupAt = newDate;
            Status = FollowupStatus.Scheduled;
        }

        public bool IsScheduled => Status == FollowupStatus.Scheduled;
        public bool IsDismissed => Status == FollowupStatus.Dismissed;
        public bool IsDone => Status == FollowupStatus.Done;
        public bool IsPast => DateTime.UtcNow > FollowupAt;
        public bool IsDue => IsScheduled && IsPast;
    }
}
