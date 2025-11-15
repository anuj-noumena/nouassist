using System;
using NouAssist.AuditTrail.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AuditTrail.Domain.Shared.Events
{
    [EventName("NouAssist.AuditTrail.EntryCreated")]
    public class AuditEntryCreatedEto : IMultiTenant
    {
        public Guid AuditEntryId { get; set; }
        public Guid? TenantId { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public AuditEventType EventType { get; set; }
        public AuditSeverity Severity { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid UserId { get; set; }
        public ComplianceFlag ComplianceFlags { get; set; }
    }
}
