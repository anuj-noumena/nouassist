using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Activity.Events
{
    [EventName("NouAssist.Activity.AuditLogCreated")]
    public class AuditLogCreatedEto : IMultiTenant
    {
        public Guid AuditLogId { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
