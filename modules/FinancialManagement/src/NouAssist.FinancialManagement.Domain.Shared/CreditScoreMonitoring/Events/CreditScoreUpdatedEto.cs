using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CreditScoreMonitoring.Domain.Shared.Events
{
    [EventName("NouAssist.CreditScoreMonitoring.CreditScoreUpdated")]
    public class CreditScoreUpdatedEto : IMultiTenant
    {
        public Guid CreditReportId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Bureau { get; set; }
        public int PreviousScore { get; set; }
        public int NewScore { get; set; }
        public int ScoreChange { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
