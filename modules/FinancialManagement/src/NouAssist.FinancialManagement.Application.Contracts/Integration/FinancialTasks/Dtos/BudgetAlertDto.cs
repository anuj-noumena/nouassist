using System;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class BudgetAlertDto
    {
        public Guid BudgetId { get; set; }
        public Guid? InvestigationTaskId { get; set; }
        public string BudgetName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal PlannedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public decimal VariancePercent { get; set; }
        public string Currency { get; set; }
        public string AlertType { get; set; } // "Variance" or "Exceeded"
        public bool IsOverBudget { get; set; }
        public DateTime DetectedAt { get; set; }
    }
}
