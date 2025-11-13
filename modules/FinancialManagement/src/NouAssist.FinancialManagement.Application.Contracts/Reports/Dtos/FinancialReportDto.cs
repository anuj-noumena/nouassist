using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;

namespace NouAssist.FinancialManagement.Reports.Dtos
{
    public class BalanceSheetDto
    {
        public DateTime ReportDate { get; set; }
        public string Currency { get; set; }
        
        public BalanceSheetSection Assets { get; set; }
        public BalanceSheetSection Liabilities { get; set; }
        public BalanceSheetSection Equity { get; set; }
        
        public MonetaryValue TotalAssets { get; set; }
        public MonetaryValue TotalLiabilities { get; set; }
        public MonetaryValue TotalEquity { get; set; }
        public bool IsBalanced => Math.Abs(TotalAssets.Amount - (TotalLiabilities.Amount + TotalEquity.Amount)) < 0.01m;
    }

    public class BalanceSheetSection
    {
        public string SectionName { get; set; }
        public List<BalanceSheetLineItem> LineItems { get; set; }
        public MonetaryValue SubTotal { get; set; }
    }

    public class BalanceSheetLineItem
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public MonetaryValue Amount { get; set; }
    }

    public class IncomeStatementDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        
        public List<IncomeStatementLineItem> Revenue { get; set; }
        public List<IncomeStatementLineItem> Expenses { get; set; }
        
        public MonetaryValue TotalRevenue { get; set; }
        public MonetaryValue TotalExpenses { get; set; }
        public MonetaryValue NetIncome { get; set; }
        public decimal NetProfitMargin { get; set; }
    }

    public class IncomeStatementLineItem
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public MonetaryValue Amount { get; set; }
    }

    public class CashFlowStatementDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        
        public CashFlowSection OperatingActivities { get; set; }
        public CashFlowSection InvestingActivities { get; set; }
        public CashFlowSection FinancingActivities { get; set; }
        
        public MonetaryValue NetCashFlow { get; set; }
        public MonetaryValue BeginningCash { get; set; }
        public MonetaryValue EndingCash { get; set; }
    }

    public class CashFlowSection
    {
        public string SectionName { get; set; }
        public List<CashFlowLineItem> LineItems { get; set; }
        public MonetaryValue NetCashChange { get; set; }
    }

    public class CashFlowLineItem
    {
        public string Description { get; set; }
        public MonetaryValue Amount { get; set; }
    }

    public class AgingReportDto
    {
        public DateTime ReportDate { get; set; }
        public string ReportType { get; set; } // "AR" or "AP"
        public List<AgingBucket> Buckets { get; set; }
        public MonetaryValue TotalOutstanding { get; set; }
    }

    public class AgingBucket
    {
        public string BucketName { get; set; } // "Current", "1-30 Days", "31-60 Days", etc.
        public int DaysFrom { get; set; }
        public int? DaysTo { get; set; }
        public List<AgingLineItem> Items { get; set; }
        public MonetaryValue SubTotal { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }

    public class AgingLineItem
    {
        public Guid EntityId { get; set; }
        public string EntityNumber { get; set; }
        public string CustomerOrVendorName { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public MonetaryValue Amount { get; set; }
    }

    public class BudgetVarianceReportDto
    {
        public Guid BudgetId { get; set; }
        public string BudgetName { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<BudgetVarianceLineItem> LineItems { get; set; }
        public MonetaryValue TotalBudgeted { get; set; }
        public MonetaryValue TotalActual { get; set; }
        public MonetaryValue TotalVariance { get; set; }
        public decimal VariancePercentage { get; set; }
    }

    public class BudgetVarianceLineItem
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public MonetaryValue Budgeted { get; set; }
        public MonetaryValue Actual { get; set; }
        public MonetaryValue Variance { get; set; }
        public decimal VariancePercentage { get; set; }
        public string Status { get; set; } // "On Track", "Warning", "Over Budget"
    }

    public class GetReportInputDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AsOfDate { get; set; }
        public string Currency { get; set; } = "USD";
        public Guid? BudgetId { get; set; }
    }
}
