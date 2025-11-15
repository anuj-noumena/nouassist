using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CreditScoreMonitoring.Domain.Shared.Events
{
    [EventName("NouAssist.CreditScoreMonitoring.CreditAlertTriggered")]
    public class CreditAlertTriggeredEto : IMultiTenant
    {
        public Guid AlertId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string AlertType { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public DateTime TriggeredAt { get; set; }
    }
}
