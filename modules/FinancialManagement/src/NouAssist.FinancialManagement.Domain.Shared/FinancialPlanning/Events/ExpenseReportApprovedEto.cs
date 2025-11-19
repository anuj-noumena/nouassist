using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.ExpenseReportApproved")]
    public class ExpenseReportApprovedEto : IMultiTenant
    {
        public Guid ReportId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid SubmitterId { get; set; }
        public Guid ApprovedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
