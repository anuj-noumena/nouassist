using System;
using System.Collections.Generic;

namespace NouAssist.FinancialManagement.Integration.SubModuleIntegration
{
    public class IntegrationDashboardDto
    {
        public GeneralLedgerSummaryDto GeneralLedgerSummary { get; set; }
        public BudgetTrackingSummaryDto BudgetTrackingSummary { get; set; }
        public IntegrationMetricsDto Metrics { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class GeneralLedgerSummaryDto
    {
        public int TotalAutoJournalEntries { get; set; }
        public int BillPaymentEntries { get; set; }
        public int InvoicePaymentEntries { get; set; }
        public int InvoiceAccrualEntries { get; set; }
        public int ExpenseEntries { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public bool IsBalanced { get; set; }
        public string Currency { get; set; }
    }

    public class BudgetTrackingSummaryDto
    {
        public int ActiveBudgets { get; set; }
        public int TotalTrackedTransactions { get; set; }
        public int VarianceAlertsCount { get; set; }
        public int OverBudgetCategories { get; set; }
        public decimal TotalBudgeted { get; set; }
        public decimal TotalActual { get; set; }
        public decimal OverallVariancePercent { get; set; }
        public string Currency { get; set; }
    }

    public class IntegrationMetricsDto
    {
        public int TotalIntegrationEvents { get; set; }
        public int SuccessfulEvents { get; set; }
        public int FailedEvents { get; set; }
        public decimal SuccessRate { get; set; }
        public TimeSpan AverageProcessingTime { get; set; }
        public DateTime LastEventProcessed { get; set; }
    }

    public class GeneralLedgerIntegrationStatusDto
    {
        public bool IsEnabled { get; set; }
        public bool IsHealthy { get; set; }
        public List<string> EnabledEventTypes { get; set; }
        public int JournalEntriesLast24Hours { get; set; }
        public List<JournalEntryMappingDto> AccountMappings { get; set; }
    }

    public class JournalEntryMappingDto
    {
        public string SourceType { get; set; }
        public Guid DebitAccountId { get; set; }
        public string DebitAccountName { get; set; }
        public Guid CreditAccountId { get; set; }
        public string CreditAccountName { get; set; }
    }

    public class BudgetIntegrationStatusDto
    {
        public bool IsEnabled { get; set; }
        public bool IsHealthy { get; set; }
        public int ActiveBudgetsCount { get; set; }
        public int TrackedTransactionsLast24Hours { get; set; }
        public List<BudgetCategorySummaryDto> Categories { get; set; }
    }

    public class BudgetCategorySummaryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Budgeted { get; set; }
        public decimal Actual { get; set; }
        public decimal VariancePercent { get; set; }
        public bool IsOverBudget { get; set; }
    }

    public class RecentJournalEntriesDto
    {
        public List<AutoJournalEntryDto> Entries { get; set; }
        public int TotalCount { get; set; }
    }

    public class AutoJournalEntryDto
    {
        public Guid BatchId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string SourceType { get; set; }
        public Guid SourceEntityId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BudgetVarianceAlertsDto
    {
        public List<BudgetVarianceAlertDto> Alerts { get; set; }
        public int TotalCount { get; set; }
    }

    public class BudgetVarianceAlertDto
    {
        public Guid BudgetId { get; set; }
        public string BudgetName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Budgeted { get; set; }
        public decimal Actual { get; set; }
        public decimal VariancePercent { get; set; }
        public bool IsOverBudget { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Severity { get; set; }
    }

    public class IntegrationHealthDto
    {
        public bool IsHealthy { get; set; }
        public List<ComponentHealthDto> Components { get; set; }
        public DateTime CheckedAt { get; set; }
    }

    public class ComponentHealthDto
    {
        public string ComponentName { get; set; }
        public bool IsHealthy { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
