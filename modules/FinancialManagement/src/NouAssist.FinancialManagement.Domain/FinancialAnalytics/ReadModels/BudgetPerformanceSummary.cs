using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;

namespace NouAssist.FinancialManagement.FinancialAnalytics.ReadModels
{
    public class BudgetPerformanceSummary
    {
        public Guid BudgetId { get; set; }
        public string BudgetName { get; set; }
        public MonetaryValue TotalBudget { get; set; }
        public MonetaryValue TotalSpent { get; set; }
        public MonetaryValue Remaining { get; set; }
        public decimal PercentageUsed { get; set; }
        public List<CategoryPerformance> CategoryBreakdown { get; set; }
        public DateTime ReportDate { get; set; }
    }

    public class CategoryPerformance
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public MonetaryValue Budgeted { get; set; }
        public MonetaryValue Actual { get; set; }
        public MonetaryValue Variance { get; set; }
        public decimal PercentageUsed { get; set; }
        public bool IsOverBudget { get; set; }
    }
}
