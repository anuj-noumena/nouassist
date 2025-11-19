using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.ExpenseReportSubmitted")]
    public class ExpenseReportSubmittedEto : IMultiTenant
    {
        public Guid ReportId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid SubmitterId { get; set; }
        public string ReportName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
