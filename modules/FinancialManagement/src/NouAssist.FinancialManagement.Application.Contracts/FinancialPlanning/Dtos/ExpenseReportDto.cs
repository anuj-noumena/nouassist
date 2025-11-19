using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class ExpenseReportDto : FullAuditedEntityDto<Guid>
    {
        public Guid SubmitterId { get; set; }
        public string ReportName { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public ReportStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public List<Guid> ExpenseIds { get; set; }
    }

    public class CreateExpenseReportDto
    {
        public string ReportName { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Currency { get; set; }
    }

    public class SubmitExpenseReportDto
    {
        public Guid ReportId { get; set; }
    }

    public class ApproveExpenseReportDto
    {
        public Guid ReportId { get; set; }
    }

    public class GetExpenseReportListDto : PagedAndSortedResultRequestDto
    {
        public Guid? SubmitterFilter { get; set; }
        public ReportStatus? StatusFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
