using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos;
using NouAssist.FinancialManagement.Integration.Events;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks
{
    [Authorize]
    public class FinancialTaskIntegrationAppService : ApplicationService, IFinancialTaskIntegrationAppService
    {
        private readonly IBillRepository _billRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IFinancialGoalRepository _goalRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ICurrentUser _currentUser;

        public FinancialTaskIntegrationAppService(
            IBillRepository billRepository,
            IInvoiceRepository invoiceRepository,
            IBudgetRepository budgetRepository,
            IFinancialGoalRepository goalRepository,
            IDistributedEventBus eventBus,
            ICurrentUser currentUser)
        {
            _billRepository = billRepository;
            _invoiceRepository = invoiceRepository;
            _budgetRepository = budgetRepository;
            _goalRepository = goalRepository;
            _eventBus = eventBus;
            _currentUser = currentUser;
        }

        public async Task<FinancialTaskDashboardDto> GetFinancialTaskDashboardAsync()
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;

            // Get all pending items
            var pendingApprovals = await GetPendingApprovalsAsync();
            var upcomingPayments = await GetUpcomingPaymentsAsync(7);
            var overdueInvoices = await GetOverdueInvoicesAsync();
            var budgetAlerts = await GetBudgetAlertsAsync();
            var goalActions = await GetGoalActionsAsync();

            // Calculate summary
            var summary = new FinancialTaskSummaryDto
            {
                PendingApprovalsCount = pendingApprovals.Count,
                PendingApprovalsTotal = pendingApprovals.Sum(x => x.Amount),
                Currency = pendingApprovals.FirstOrDefault()?.Currency ?? "USD",
                UpcomingPaymentsCount = upcomingPayments.Count,
                UpcomingPaymentsTotal = upcomingPayments.Sum(x => x.Amount),
                OverdueInvoicesCount = overdueInvoices.Count,
                OverdueInvoicesTotal = overdueInvoices.Sum(x => x.AmountDue),
                BudgetAlertsCount = budgetAlerts.Count,
                ActiveGoalsCount = goalActions.Count,
                OverdueTasksCount = 0 // Would calculate from task repository
            };

            return new FinancialTaskDashboardDto
            {
                Summary = summary,
                PendingApprovals = pendingApprovals.Take(5).ToList(),
                UpcomingPayments = upcomingPayments.Take(5).ToList(),
                OverdueInvoices = overdueInvoices.Take(5).ToList(),
                BudgetAlerts = budgetAlerts.Take(5).ToList(),
                GoalActions = goalActions.Take(5).ToList()
            };
        }

        public async Task<List<PendingBillApprovalDto>> GetPendingApprovalsAsync()
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;

            // Get all bills pending approval
            var pendingBills = await _billRepository.GetListAsync(
                bill => bill.Status == BillStatus.PendingApproval &&
                        (tenantId == null || bill.TenantId == tenantId));

            return pendingBills.Select(bill => new PendingBillApprovalDto
            {
                BillId = bill.Id,
                BillNumber = bill.BillNumber,
                VendorName = bill.VendorName,
                Amount = bill.TotalAmount.Amount,
                Currency = bill.TotalAmount.Currency,
                DueDate = bill.DueDate,
                TaskDueDate = bill.DueDate.AddDays(-2), // Task due 2 days before bill
                DaysUntilDue = (bill.DueDate - DateTime.Today).Days,
                Priority = GetPriorityLabel(bill.TotalAmount.Amount),
                CategoryId = bill.CategoryId,
                CategoryName = "Category", // Would load from repository
                ProjectId = bill.ProjectId,
                ProjectName = null // Would load from project repository
            }).OrderBy(x => x.DaysUntilDue).ToList();
        }

        public async Task<List<UpcomingBillPaymentDto>> GetUpcomingPaymentsAsync(int daysAhead = 7)
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;
            var cutoffDate = DateTime.Today.AddDays(daysAhead);

            // Get bills due within the specified timeframe
            var upcomingBills = await _billRepository.GetListAsync(
                bill => (bill.Status == BillStatus.Approved || bill.Status == BillStatus.PendingPayment) &&
                        bill.DueDate <= cutoffDate &&
                        (tenantId == null || bill.TenantId == tenantId));

            return upcomingBills.Select(bill => new UpcomingBillPaymentDto
            {
                BillId = bill.Id,
                BillNumber = bill.BillNumber,
                VendorName = bill.VendorName,
                Amount = bill.TotalAmount.Amount,
                Currency = bill.TotalAmount.Currency,
                DueDate = bill.DueDate,
                TaskDueDate = bill.DueDate.AddDays(-1), // Task due 1 day before bill
                DaysUntilDue = (bill.DueDate - DateTime.Today).Days,
                Status = bill.Status.ToString(),
                IsApproved = bill.Status == BillStatus.Approved,
                PaymentAccountId = null // Would be set when payment scheduled
            }).OrderBy(x => x.DaysUntilDue).ToList();
        }

        public async Task<List<OverdueInvoiceDto>> GetOverdueInvoicesAsync()
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;
            var today = DateTime.Today;

            // Get overdue invoices
            var overdueInvoices = await _invoiceRepository.GetListAsync(
                invoice => (invoice.Status == InvoiceStatus.Sent || invoice.Status == InvoiceStatus.PartiallyPaid) &&
                           invoice.DueDate < today &&
                           (tenantId == null || invoice.TenantId == tenantId));

            return overdueInvoices.Select(invoice =>
            {
                var amountDue = invoice.TotalAmount.Subtract(invoice.PaidAmount);
                var daysOverdue = (today - invoice.DueDate).Days;

                return new OverdueInvoiceDto
                {
                    InvoiceId = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    CustomerName = invoice.CustomerName,
                    TotalAmount = invoice.TotalAmount.Amount,
                    AmountDue = amountDue.Amount,
                    Currency = invoice.TotalAmount.Currency,
                    DueDate = invoice.DueDate,
                    DaysOverdue = daysOverdue,
                    LastContactDate = null, // Would track in invoice metadata
                    CollectionPriority = GetCollectionPriority(daysOverdue),
                    CollectionAttempts = 0 // Would track in invoice metadata
                };
            }).OrderByDescending(x => x.DaysOverdue).ToList();
        }

        public async Task<List<BudgetAlertDto>> GetBudgetAlertsAsync()
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;

            // Get active budgets
            var activeBudgets = await _budgetRepository.GetListAsync(
                budget => budget.Status == BudgetStatus.Active &&
                          budget.UserId == userId &&
                          (tenantId == null || budget.TenantId == tenantId));

            var alerts = new List<BudgetAlertDto>();

            foreach (var budget in activeBudgets)
            {
                foreach (var allocation in budget.Allocations)
                {
                    var variance = allocation.Actual.Subtract(allocation.Allocated);
                    var variancePercent = allocation.Allocated.Amount > 0
                        ? (variance.Amount / allocation.Allocated.Amount) * 100
                        : 0;

                    // Only include if variance is significant (>10%)
                    if (Math.Abs(variancePercent) > 10)
                    {
                        alerts.Add(new BudgetAlertDto
                        {
                            BudgetId = budget.Id,
                            BudgetName = budget.Name,
                            CategoryId = allocation.CategoryId,
                            CategoryName = "Category", // Would load from repository
                            PlannedAmount = allocation.Allocated.Amount,
                            ActualAmount = allocation.Actual.Amount,
                            Variance = variance.Amount,
                            VariancePercent = variancePercent,
                            Currency = allocation.Allocated.Currency,
                            AlertType = variancePercent > 0 ? "Exceeded" : "Variance",
                            IsOverBudget = variancePercent > 0,
                            DetectedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            return alerts.OrderByDescending(x => Math.Abs(x.VariancePercent)).ToList();
        }

        public async Task<List<GoalActionItemDto>> GetGoalActionsAsync()
        {
            var userId = _currentUser.GetId();
            var tenantId = _currentUser.TenantId;

            // Get active goals
            var activeGoals = await _goalRepository.GetListAsync(
                goal => goal.Status == GoalStatus.Active &&
                        goal.UserId == userId &&
                        (tenantId == null || goal.TenantId == tenantId));

            return activeGoals.Select(goal =>
            {
                var remaining = goal.TargetAmount.Subtract(goal.CurrentAmount);
                var progressPercent = goal.TargetAmount.Amount > 0
                    ? (goal.CurrentAmount.Amount / goal.TargetAmount.Amount) * 100
                    : 0;

                return new GoalActionItemDto
                {
                    GoalId = goal.Id,
                    GoalName = goal.Name,
                    GoalType = goal.Type.ToString(),
                    TargetAmount = goal.TargetAmount.Amount,
                    CurrentAmount = goal.CurrentAmount.Amount,
                    RemainingAmount = remaining.Amount,
                    ProgressPercent = progressPercent,
                    Currency = goal.TargetAmount.Currency,
                    TargetDate = goal.TargetDate,
                    DaysRemaining = (goal.TargetDate - DateTime.Today).Days,
                    ActionType = "CheckIn",
                    TaskDueDate = DateTime.Today.AddDays(7),
                    TaskStatus = "Pending"
                };
            }).OrderBy(x => x.DaysRemaining).ToList();
        }

        public async Task TriggerBillApprovalWorkflowAsync(Guid billId)
        {
            var bill = await _billRepository.GetAsync(billId);

            // Publish BillCreatedEto to trigger workflow
            await _eventBus.PublishAsync(new BillCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BillId = bill.Id,
                TenantId = bill.TenantId,
                BillNumber = bill.BillNumber,
                VendorId = bill.VendorId,
                VendorName = bill.VendorName,
                Amount = bill.TotalAmount,
                DueDate = bill.DueDate,
                CategoryId = bill.CategoryId,
                ProjectId = bill.ProjectId,
                RequiresApproval = bill.RequiresApproval,
                CreatedBy = _currentUser.GetId()
            });
        }

        public async Task TriggerInvoiceCollectionWorkflowAsync(Guid invoiceId)
        {
            var invoice = await _invoiceRepository.GetAsync(invoiceId);
            var amountDue = invoice.TotalAmount.Subtract(invoice.PaidAmount);
            var daysOverdue = (DateTime.Today - invoice.DueDate).Days;

            if (daysOverdue > 0)
            {
                // Publish InvoiceOverdueEto to trigger collection workflow
                await _eventBus.PublishAsync(new InvoiceOverdueEto
                {
                    InvoiceId = invoice.Id,
                    TenantId = invoice.TenantId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    CustomerId = invoice.CustomerId,
                    CustomerName = invoice.CustomerName,
                    TotalAmount = invoice.TotalAmount,
                    AmountDue = amountDue,
                    DueDate = invoice.DueDate,
                    DaysOverdue = daysOverdue,
                    ProjectId = invoice.ProjectId,
                    LastContactDate = null,
                    OverdueDetectedAt = DateTime.UtcNow
                });
            }
        }

        public async Task TriggerBudgetReviewWorkflowAsync(Guid budgetId)
        {
            var budget = await _budgetRepository.GetAsync(budgetId);

            // Analyze budget and create variance events for significant deviations
            foreach (var allocation in budget.Allocations)
            {
                var variance = allocation.Actual.Subtract(allocation.Allocated);
                var variancePercent = allocation.Allocated.Amount > 0
                    ? (variance.Amount / allocation.Allocated.Amount) * 100
                    : 0;

                if (Math.Abs(variancePercent) > 10)
                {
                    await _eventBus.PublishAsync(new BudgetVarianceDetectedEto
                    {
                        BudgetId = budget.Id,
                        TenantId = budget.TenantId,
                        UserId = budget.UserId,
                        BudgetName = budget.Name,
                        CategoryId = allocation.CategoryId,
                        CategoryName = "Category", // Would load from repository
                        PlannedAmount = allocation.Allocated,
                        ActualAmount = allocation.Actual,
                        Variance = variance,
                        VariancePercent = variancePercent,
                        IsOverBudget = variancePercent > 0,
                        DetectedAt = DateTime.UtcNow
                    });
                }
            }
        }

        #region Private Helpers

        private string GetPriorityLabel(decimal amount)
        {
            if (amount > 1000) return "High";
            if (amount > 500) return "Medium";
            return "Low";
        }

        private string GetCollectionPriority(int daysOverdue)
        {
            if (daysOverdue > 14) return "Urgent";
            if (daysOverdue > 7) return "High";
            return "Medium";
        }

        #endregion
    }
}
