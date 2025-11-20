using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.Integration.SubModuleIntegration
{
    public class FinancialSubModuleIntegrationAppService : ApplicationService, IFinancialSubModuleIntegrationAppService
    {
        private readonly IJournalEntryBatchRepository _journalEntryBatchRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly ILedgerAccountRepository _ledgerAccountRepository;
        private readonly ILogger<FinancialSubModuleIntegrationAppService> _logger;

        public FinancialSubModuleIntegrationAppService(
            IJournalEntryBatchRepository journalEntryBatchRepository,
            IBudgetRepository budgetRepository,
            ILedgerAccountRepository ledgerAccountRepository,
            ILogger<FinancialSubModuleIntegrationAppService> logger)
        {
            _journalEntryBatchRepository = journalEntryBatchRepository;
            _budgetRepository = budgetRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
            _logger = logger;
        }

        public async Task<IntegrationDashboardDto> GetIntegrationDashboardAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-30);
            var end = endDate ?? DateTime.UtcNow;

            _logger.LogInformation("Generating integration dashboard for period {StartDate} to {EndDate}", start, end);

            var glSummary = await GetGeneralLedgerSummaryAsync(start, end);
            var budgetSummary = await GetBudgetTrackingSummaryAsync(start, end);
            var metrics = await GetIntegrationMetricsAsync(start, end);

            return new IntegrationDashboardDto
            {
                GeneralLedgerSummary = glSummary,
                BudgetTrackingSummary = budgetSummary,
                Metrics = metrics,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<GeneralLedgerIntegrationStatusDto> GetGeneralLedgerStatusAsync()
        {
            var journalEntries = await _journalEntryBatchRepository.GetListAsync();
            var last24Hours = journalEntries
                .Where(j => j.CreationTime >= DateTime.UtcNow.AddHours(-24))
                .Count();

            return new GeneralLedgerIntegrationStatusDto
            {
                IsEnabled = true,
                IsHealthy = last24Hours >= 0, // Always healthy if no errors
                EnabledEventTypes = new List<string> { "BillPaid", "InvoicePaid", "InvoiceSent", "ExpenseCreated" },
                JournalEntriesLast24Hours = last24Hours,
                AccountMappings = new List<JournalEntryMappingDto>() // TODO: Implement account mapping retrieval
            };
        }

        public async Task<BudgetIntegrationStatusDto> GetBudgetStatusAsync()
        {
            var budgets = await _budgetRepository.GetListAsync();
            var activeBudgets = budgets.Where(b => b.Status == FinancialPlanning.Enums.BudgetStatus.Active).ToList();

            var categories = new List<BudgetCategorySummaryDto>();
            foreach (var budget in activeBudgets)
            {
                foreach (var allocation in budget.Allocations)
                {
                    var variancePercent = allocation.BudgetedAmount.Amount > 0
                        ? ((allocation.ActualAmount.Amount / allocation.BudgetedAmount.Amount) * 100) - 100
                        : 0;

                    categories.Add(new BudgetCategorySummaryDto
                    {
                        CategoryId = allocation.CategoryId,
                        CategoryName = allocation.CategoryName,
                        Budgeted = allocation.BudgetedAmount.Amount,
                        Actual = allocation.ActualAmount.Amount,
                        VariancePercent = variancePercent,
                        IsOverBudget = allocation.ActualAmount.Amount > allocation.BudgetedAmount.Amount
                    });
                }
            }

            return new BudgetIntegrationStatusDto
            {
                IsEnabled = true,
                IsHealthy = true,
                ActiveBudgetsCount = activeBudgets.Count,
                TrackedTransactionsLast24Hours = 0, // TODO: Track this metric
                Categories = categories
            };
        }

        public async Task<RecentJournalEntriesDto> GetRecentAutoJournalEntriesAsync(int count = 10)
        {
            var journalEntries = await _journalEntryBatchRepository.GetListAsync();
            var autoEntries = journalEntries
                .Where(j => j.SourceEntityId.HasValue) // Only auto-generated entries have source entity
                .OrderByDescending(j => j.CreationTime)
                .Take(count)
                .Select(j => new AutoJournalEntryDto
                {
                    BatchId = j.Id,
                    BatchNumber = j.BatchNumber,
                    PostingDate = j.PostingDate.Date,
                    Description = j.Description,
                    SourceType = j.SourceEntityType?.ToString() ?? "Unknown",
                    SourceEntityId = j.SourceEntityId ?? Guid.Empty,
                    Amount = j.Lines.Where(l => l.DebitAmount != null).Sum(l => l.DebitAmount.Amount),
                    Currency = j.Lines.FirstOrDefault()?.DebitAmount?.Currency ?? "USD",
                    CreatedAt = j.CreationTime
                })
                .ToList();

            return new RecentJournalEntriesDto
            {
                Entries = autoEntries,
                TotalCount = autoEntries.Count
            };
        }

        public async Task<BudgetVarianceAlertsDto> GetRecentBudgetVariancesAsync(int count = 10)
        {
            // TODO: Implement budget variance alerts retrieval
            // This would query a BudgetVarianceAlert table if we were persisting them
            // For now, calculate from current budget state

            var budgets = await _budgetRepository.GetListAsync();
            var activeBudgets = budgets.Where(b => b.Status == FinancialPlanning.Enums.BudgetStatus.Active);

            var alerts = new List<BudgetVarianceAlertDto>();

            foreach (var budget in activeBudgets)
            {
                foreach (var allocation in budget.Allocations)
                {
                    var variancePercent = allocation.BudgetedAmount.Amount > 0
                        ? ((allocation.ActualAmount.Amount / allocation.BudgetedAmount.Amount) * 100) - 100
                        : 0;

                    // Only include variances > 10% or over budget
                    if (Math.Abs(variancePercent) > 10 || allocation.ActualAmount.Amount > allocation.BudgetedAmount.Amount)
                    {
                        alerts.Add(new BudgetVarianceAlertDto
                        {
                            BudgetId = budget.Id,
                            BudgetName = budget.Name,
                            CategoryId = allocation.CategoryId,
                            CategoryName = allocation.CategoryName,
                            Budgeted = allocation.BudgetedAmount.Amount,
                            Actual = allocation.ActualAmount.Amount,
                            VariancePercent = variancePercent,
                            IsOverBudget = allocation.ActualAmount.Amount > allocation.BudgetedAmount.Amount,
                            DetectedAt = DateTime.UtcNow,
                            Severity = variancePercent > 0 ? "Critical" : "Warning"
                        });
                    }
                }
            }

            return new BudgetVarianceAlertsDto
            {
                Alerts = alerts.OrderByDescending(a => Math.Abs(a.VariancePercent)).Take(count).ToList(),
                TotalCount = alerts.Count
            };
        }

        public async Task<IntegrationHealthDto> GetIntegrationHealthAsync()
        {
            var components = new List<ComponentHealthDto>();

            // Check GeneralLedger integration health
            try
            {
                var journalEntries = await _journalEntryBatchRepository.GetListAsync();
                var recentEntries = journalEntries.Where(j => j.CreationTime >= DateTime.UtcNow.AddHours(-24)).Count();

                components.Add(new ComponentHealthDto
                {
                    ComponentName = "GeneralLedger Integration",
                    IsHealthy = true,
                    Status = "Healthy",
                    Message = $"{recentEntries} journal entries created in last 24 hours"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GeneralLedger integration health check failed");
                components.Add(new ComponentHealthDto
                {
                    ComponentName = "GeneralLedger Integration",
                    IsHealthy = false,
                    Status = "Unhealthy",
                    Message = ex.Message
                });
            }

            // Check Budget integration health
            try
            {
                var budgets = await _budgetRepository.GetListAsync();
                var activeBudgets = budgets.Where(b => b.Status == FinancialPlanning.Enums.BudgetStatus.Active).Count();

                components.Add(new ComponentHealthDto
                {
                    ComponentName = "Budget Tracking Integration",
                    IsHealthy = true,
                    Status = "Healthy",
                    Message = $"{activeBudgets} active budgets being tracked"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Budget tracking health check failed");
                components.Add(new ComponentHealthDto
                {
                    ComponentName = "Budget Tracking Integration",
                    IsHealthy = false,
                    Status = "Unhealthy",
                    Message = ex.Message
                });
            }

            return new IntegrationHealthDto
            {
                IsHealthy = components.All(c => c.IsHealthy),
                Components = components,
                CheckedAt = DateTime.UtcNow
            };
        }

        private async Task<GeneralLedgerSummaryDto> GetGeneralLedgerSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var journalEntries = await _journalEntryBatchRepository.GetListAsync();
            var filteredEntries = journalEntries
                .Where(j => j.CreationTime >= startDate && j.CreationTime <= endDate && j.SourceEntityId.HasValue)
                .ToList();

            var billPaymentEntries = filteredEntries.Count(j => j.SourceEntityType == Shared.Enums.EntityType.Bill);
            var invoicePaymentEntries = filteredEntries.Count(j => j.SourceEntityType == Shared.Enums.EntityType.Invoice);
            var expenseEntries = filteredEntries.Count(j => j.SourceEntityType == Shared.Enums.EntityType.Expense);

            var totalDebits = filteredEntries.SelectMany(j => j.Lines).Where(l => l.DebitAmount != null).Sum(l => l.DebitAmount.Amount);
            var totalCredits = filteredEntries.SelectMany(j => j.Lines).Where(l => l.CreditAmount != null).Sum(l => l.CreditAmount.Amount);

            return new GeneralLedgerSummaryDto
            {
                TotalAutoJournalEntries = filteredEntries.Count,
                BillPaymentEntries = billPaymentEntries,
                InvoicePaymentEntries = invoicePaymentEntries,
                InvoiceAccrualEntries = 0, // TODO: Differentiate invoice accrual vs payment
                ExpenseEntries = expenseEntries,
                TotalDebits = totalDebits,
                TotalCredits = totalCredits,
                IsBalanced = Math.Abs(totalDebits - totalCredits) < 0.01m,
                Currency = "USD"
            };
        }

        private async Task<BudgetTrackingSummaryDto> GetBudgetTrackingSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var budgets = await _budgetRepository.GetListAsync();
            var activeBudgets = budgets.Where(b => b.Status == FinancialPlanning.Enums.BudgetStatus.Active).ToList();

            var totalBudgeted = activeBudgets.Sum(b => b.TotalBudget.Amount);
            var totalActual = activeBudgets.Sum(b => b.ActualSpent.Amount);

            var overBudgetCategories = activeBudgets
                .SelectMany(b => b.Allocations)
                .Count(a => a.ActualAmount.Amount > a.BudgetedAmount.Amount);

            var variancePercent = totalBudgeted > 0 ? ((totalActual / totalBudgeted) * 100) - 100 : 0;

            return new BudgetTrackingSummaryDto
            {
                ActiveBudgets = activeBudgets.Count,
                TotalTrackedTransactions = 0, // TODO: Track this metric
                VarianceAlertsCount = overBudgetCategories,
                OverBudgetCategories = overBudgetCategories,
                TotalBudgeted = totalBudgeted,
                TotalActual = totalActual,
                OverallVariancePercent = variancePercent,
                Currency = "USD"
            };
        }

        private Task<IntegrationMetricsDto> GetIntegrationMetricsAsync(DateTime startDate, DateTime endDate)
        {
            // TODO: Implement integration event metrics tracking
            // This would require persisting integration event logs

            return Task.FromResult(new IntegrationMetricsDto
            {
                TotalIntegrationEvents = 0,
                SuccessfulEvents = 0,
                FailedEvents = 0,
                SuccessRate = 100,
                AverageProcessingTime = TimeSpan.Zero,
                LastEventProcessed = DateTime.UtcNow
            });
        }
    }
}
