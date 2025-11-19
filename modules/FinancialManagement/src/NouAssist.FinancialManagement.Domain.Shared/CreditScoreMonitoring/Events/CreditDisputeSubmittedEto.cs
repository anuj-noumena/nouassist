using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Events
{
    [EventName("NouAssist.CreditScoreMonitoring.CreditDisputeSubmitted")]
    public class CreditDisputeSubmittedEto : IMultiTenant
    {
        public Guid DisputeId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Bureau { get; set; }
        public string ItemType { get; set; }
        public string Reason { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
