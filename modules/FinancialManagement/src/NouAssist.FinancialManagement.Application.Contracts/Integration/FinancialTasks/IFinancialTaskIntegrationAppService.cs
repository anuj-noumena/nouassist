using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks
{
    public interface IFinancialTaskIntegrationAppService : IApplicationService
    {
        /// <summary>
        /// Get comprehensive financial task dashboard
        /// </summary>
        Task<FinancialTaskDashboardDto> GetFinancialTaskDashboardAsync();

        /// <summary>
        /// Get pending bill approvals
        /// </summary>
        Task<List<PendingBillApprovalDto>> GetPendingApprovalsAsync();

        /// <summary>
        /// Get upcoming bill payments
        /// </summary>
        Task<List<UpcomingBillPaymentDto>> GetUpcomingPaymentsAsync(int daysAhead = 7);

        /// <summary>
        /// Get overdue invoices requiring collection
        /// </summary>
        Task<List<OverdueInvoiceDto>> GetOverdueInvoicesAsync();

        /// <summary>
        /// Get budget alerts (variances and overruns)
        /// </summary>
        Task<List<BudgetAlertDto>> GetBudgetAlertsAsync();

        /// <summary>
        /// Get goal action items
        /// </summary>
        Task<List<GoalActionItemDto>> GetGoalActionsAsync();

        /// <summary>
        /// Manually trigger bill approval workflow
        /// </summary>
        Task TriggerBillApprovalWorkflowAsync(Guid billId);

        /// <summary>
        /// Manually trigger invoice collection workflow
        /// </summary>
        Task TriggerInvoiceCollectionWorkflowAsync(Guid invoiceId);

        /// <summary>
        /// Manually trigger budget review workflow
        /// </summary>
        Task TriggerBudgetReviewWorkflowAsync(Guid budgetId);
    }
}
