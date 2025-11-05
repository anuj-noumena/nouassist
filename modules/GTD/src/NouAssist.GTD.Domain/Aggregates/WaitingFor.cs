using System;
using NouAssist.Core.Enums;
using NouAssist.GTD.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.GTD.Aggregates
{
    public class WaitingFor : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TaskInstanceId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public WaitingForType WfType { get; private set; }
        public Guid? ForUserId { get; private set; }
        public string? ExternalReferenceJson { get; set; }
        public string? Reason { get; set; }
        public WaitingForStatus Status { get; private set; }
        public string? Note { get; set; }
        public DateTime? ClearedAt { get; private set; }
        public DateTime? FollowUpDate { get; set; }

        protected WaitingFor() { }

        public WaitingFor(
            Guid id, 
            Guid ownerId, 
            WaitingForType wfType = WaitingForType.User,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            WfType = wfType;
            TenantId = tenantId;
            Status = WaitingForStatus.Active;
        }

        public void Resolve()
        {
            if (Status == WaitingForStatus.Resolved) return;

            Status = WaitingForStatus.Resolved;
            ClearedAt = DateTime.UtcNow;
        }

        public void PutOnHold()
        {
            Status = WaitingForStatus.OnHold;
        }

        public void Reactivate()
        {
            Status = WaitingForStatus.Active;
        }

        public void Cancel()
        {
            Status = WaitingForStatus.Cancelled;
            ClearedAt = DateTime.UtcNow;
        }

        public void Escalate()
        {
            Status = WaitingForStatus.Escalated;
        }

        public void AddNote(string note)
        {
            Note = note;
        }

        public void SetFollowUp(DateTime date)
        {
            FollowUpDate = date;
        }

        public bool IsActive => Status == WaitingForStatus.Active;
        public bool IsResolved => Status == WaitingForStatus.Resolved;
        public bool IsCleared => ClearedAt.HasValue;
        public bool HasFollowUp => FollowUpDate.HasValue;
        
        public bool IsFollowUpDue => 
            FollowUpDate.HasValue && DateTime.UtcNow >= FollowUpDate.Value;

        public TimeSpan WaitingDuration => 
            (ClearedAt ?? DateTime.UtcNow) - CreationTime;

        public int WaitingDays => WaitingDuration.Days;
    }
}
