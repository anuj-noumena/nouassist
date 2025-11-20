using System.Collections.Generic;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class FinancialTaskDashboardDto
    {
        public FinancialTaskSummaryDto Summary { get; set; }
        public List<PendingBillApprovalDto> PendingApprovals { get; set; }
        public List<UpcomingBillPaymentDto> UpcomingPayments { get; set; }
        public List<OverdueInvoiceDto> OverdueInvoices { get; set; }
        public List<BudgetAlertDto> BudgetAlerts { get; set; }
        public List<GoalActionItemDto> GoalActions { get; set; }
    }

    public class FinancialTaskSummaryDto
    {
        public int PendingApprovalsCount { get; set; }
        public decimal PendingApprovalsTotal { get; set; }
        public string Currency { get; set; }

        public int UpcomingPaymentsCount { get; set; }
        public decimal UpcomingPaymentsTotal { get; set; }

        public int OverdueInvoicesCount { get; set; }
        public decimal OverdueInvoicesTotal { get; set; }

        public int BudgetAlertsCount { get; set; }
        public int ActiveGoalsCount { get; set; }
        public int OverdueTasksCount { get; set; }
    }
}
