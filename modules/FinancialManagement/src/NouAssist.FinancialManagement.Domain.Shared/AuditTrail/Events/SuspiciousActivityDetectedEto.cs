using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AuditTrail.Events
{
    [EventName("NouAssist.AuditTrail.SuspiciousActivityDetected")]
    public class SuspiciousActivityDetectedEto : IMultiTenant
    {
        public Guid AlertId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ActivityDescription { get; set; }
        public List<string> SuspiciousPatterns { get; set; }
        public DateTime DetectedAt { get; set; }
        public Dictionary<string, object> Context { get; set; }
    }
}
