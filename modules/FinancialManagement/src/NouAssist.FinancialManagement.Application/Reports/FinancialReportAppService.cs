using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Reports.Dtos;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Reports
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.ViewFinancialReports)]
    public class FinancialReportAppService : ApplicationService, IFinancialReportAppService
    {
        private readonly ILedgerAccountRepository _ledgerAccountRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBillRepository _billRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IExpenseRepository _expenseRepository;

        public FinancialReportAppService(
            ILedgerAccountRepository ledgerAccountRepository,
            IInvoiceRepository invoiceRepository,
            IBillRepository billRepository,
            IBudgetRepository budgetRepository,
            IExpenseRepository expenseRepository)
        {
            _ledgerAccountRepository = ledgerAccountRepository;
            _invoiceRepository = invoiceRepository;
            _billRepository = billRepository;
            _budgetRepository = budgetRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<BalanceSheetDto> GetBalanceSheetAsync(GetReportInputDto input)
        {
            var reportDate = input.AsOfDate ?? DateTime.Today;
            var allAccounts = await _ledgerAccountRepository.GetAllActiveAccountsAsync();

            var balanceSheet = new BalanceSheetDto
            {
                ReportDate = reportDate,
                Currency = input.Currency,
                Assets = new BalanceSheetSection { SectionName = "Assets", LineItems = new List<BalanceSheetLineItem>() },
                Liabilities = new BalanceSheetSection { SectionName = "Liabilities", LineItems = new List<BalanceSheetLineItem>() },
                Equity = new BalanceSheetSection { SectionName = "Equity", LineItems = new List<BalanceSheetLineItem>() }
            };

            foreach (var account in allAccounts)
            {
                var lineItem = new BalanceSheetLineItem
                {
                    AccountNumber = account.AccountNumber.Number,
                    AccountName = account.Name,
                    Amount = account.CurrentBalance
                };

                switch (account.Type)
                {
                    case AccountType.Asset:
                        balanceSheet.Assets.LineItems.Add(lineItem);
                        break;
                    case AccountType.Liability:
                        balanceSheet.Liabilities.LineItems.Add(lineItem);
                        break;
                    case AccountType.Equity:
                        balanceSheet.Equity.LineItems.Add(lineItem);
                        break;
                }
            }

            // Calculate totals
            balanceSheet.Assets.SubTotal = new MonetaryValue(
                balanceSheet.Assets.LineItems.Sum(l => l.Amount.Amount),
                input.Currency);

            balanceSheet.Liabilities.SubTotal = new MonetaryValue(
                balanceSheet.Liabilities.LineItems.Sum(l => l.Amount.Amount),
                input.Currency);

            balanceSheet.Equity.SubTotal = new MonetaryValue(
                balanceSheet.Equity.LineItems.Sum(l => l.Amount.Amount),
                input.Currency);

            balanceSheet.TotalAssets = balanceSheet.Assets.SubTotal;
            balanceSheet.TotalLiabilities = balanceSheet.Liabilities.SubTotal;
            balanceSheet.TotalEquity = balanceSheet.Equity.SubTotal;

            return balanceSheet;
        }

        public async Task<IncomeStatementDto> GetIncomeStatementAsync(GetReportInputDto input)
        {
            var startDate = input.StartDate ?? new DateTime(DateTime.Today.Year, 1, 1);
            var endDate = input.EndDate ?? DateTime.Today;

            var allAccounts = await _ledgerAccountRepository.GetAllActiveAccountsAsync();

            var incomeStatement = new IncomeStatementDto
            {
                StartDate = startDate,
                EndDate = endDate,
                Currency = input.Currency,
                Revenue = new List<IncomeStatementLineItem>(),
                Expenses = new List<IncomeStatementLineItem>()
            };

            foreach (var account in allAccounts)
            {
                if (account.Type == AccountType.Revenue)
                {
                    incomeStatement.Revenue.Add(new IncomeStatementLineItem
                    {
                        AccountNumber = account.AccountNumber.Number,
                        AccountName = account.Name,
                        Amount = account.CurrentBalance
                    });
                }
                else if (account.Type == AccountType.Expense)
                {
                    incomeStatement.Expenses.Add(new IncomeStatementLineItem
                    {
                        AccountNumber = account.AccountNumber.Number,
                        AccountName = account.Name,
                        Amount = account.CurrentBalance
                    });
                }
            }

            // Calculate totals
            incomeStatement.TotalRevenue = new MonetaryValue(
                incomeStatement.Revenue.Sum(l => l.Amount.Amount),
                input.Currency);

            incomeStatement.TotalExpenses = new MonetaryValue(
                incomeStatement.Expenses.Sum(l => l.Amount.Amount),
                input.Currency);

            incomeStatement.NetIncome = new MonetaryValue(
                incomeStatement.TotalRevenue.Amount - incomeStatement.TotalExpenses.Amount,
                input.Currency);

            incomeStatement.NetProfitMargin = incomeStatement.TotalRevenue.Amount != 0
                ? (incomeStatement.NetIncome.Amount / incomeStatement.TotalRevenue.Amount) * 100
                : 0;

            return incomeStatement;
        }

        public async Task<CashFlowStatementDto> GetCashFlowStatementAsync(GetReportInputDto input)
        {
            var startDate = input.StartDate ?? new DateTime(DateTime.Today.Year, 1, 1);
            var endDate = input.EndDate ?? DateTime.Today;

            // This is a simplified implementation
            // In real scenario, you'd analyze journal entries and categorize them
            var cashFlowStatement = new CashFlowStatementDto
            {
                StartDate = startDate,
                EndDate = endDate,
                Currency = input.Currency,
                OperatingActivities = new CashFlowSection
                {
                    SectionName = "Operating Activities",
                    LineItems = new List<CashFlowLineItem>()
                },
                InvestingActivities = new CashFlowSection
                {
                    SectionName = "Investing Activities",
                    LineItems = new List<CashFlowLineItem>()
                },
                FinancingActivities = new CashFlowSection
                {
                    SectionName = "Financing Activities",
                    LineItems = new List<CashFlowLineItem>()
                }
            };

            // Calculate cash account changes
            var cashAccounts = await _ledgerAccountRepository.GetByTypeAsync(AccountType.Asset);
            var cashAccount = cashAccounts.FirstOrDefault(a => a.Name.Contains("Cash", StringComparison.OrdinalIgnoreCase));

            if (cashAccount != null)
            {
                cashFlowStatement.BeginningCash = new MonetaryValue(0, input.Currency); // Should be calculated
                cashFlowStatement.EndingCash = cashAccount.CurrentBalance;
            }

            return cashFlowStatement;
        }

        public async Task<AgingReportDto> GetAccountsReceivableAgingAsync(GetReportInputDto input)
        {
            var reportDate = input.AsOfDate ?? DateTime.Today;
            var overdueInvoices = await _invoiceRepository.GetOverdueInvoicesAsync(CurrentTenant.Id);

            var report = new AgingReportDto
            {
                ReportDate = reportDate,
                ReportType = "AR",
                Buckets = new List<AgingBucket>
                {
                    new AgingBucket { BucketName = "Current", DaysFrom = 0, DaysTo = 0, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "1-30 Days", DaysFrom = 1, DaysTo = 30, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "31-60 Days", DaysFrom = 31, DaysTo = 60, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "61-90 Days", DaysFrom = 61, DaysTo = 90, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "Over 90 Days", DaysFrom = 91, DaysTo = null, Items = new List<AgingLineItem>() }
                }
            };

            foreach (var invoice in overdueInvoices)
            {
                var daysOverdue = (reportDate - invoice.DueDate).Days;
                var bucket = report.Buckets.FirstOrDefault(b => 
                    daysOverdue >= b.DaysFrom && (!b.DaysTo.HasValue || daysOverdue <= b.DaysTo.Value));

                if (bucket != null)
                {
                    bucket.Items.Add(new AgingLineItem
                    {
                        EntityId = invoice.Id,
                        EntityNumber = invoice.InvoiceNumber,
                        CustomerOrVendorName = "Customer", // Load from customer repository
                        DueDate = invoice.DueDate,
                        DaysOverdue = daysOverdue,
                        Amount = invoice.GetRemainingBalance()
                    });
                }
            }

            // Calculate totals
            foreach (var bucket in report.Buckets)
            {
                bucket.SubTotal = new MonetaryValue(
                    bucket.Items.Sum(i => i.Amount.Amount),
                    input.Currency);
            }

            report.TotalOutstanding = new MonetaryValue(
                report.Buckets.Sum(b => b.SubTotal.Amount),
                input.Currency);

            // Calculate percentages
            foreach (var bucket in report.Buckets)
            {
                bucket.PercentageOfTotal = report.TotalOutstanding.Amount != 0
                    ? (bucket.SubTotal.Amount / report.TotalOutstanding.Amount) * 100
                    : 0;
            }

            return report;
        }

        public async Task<AgingReportDto> GetAccountsPayableAgingAsync(GetReportInputDto input)
        {
            var reportDate = input.AsOfDate ?? DateTime.Today;
            var overdueBills = await _billRepository.GetOverdueBillsAsync(CurrentTenant.Id);

            var report = new AgingReportDto
            {
                ReportDate = reportDate,
                ReportType = "AP",
                Buckets = new List<AgingBucket>
                {
                    new AgingBucket { BucketName = "Current", DaysFrom = 0, DaysTo = 0, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "1-30 Days", DaysFrom = 1, DaysTo = 30, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "31-60 Days", DaysFrom = 31, DaysTo = 60, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "61-90 Days", DaysFrom = 61, DaysTo = 90, Items = new List<AgingLineItem>() },
                    new AgingBucket { BucketName = "Over 90 Days", DaysFrom = 91, DaysTo = null, Items = new List<AgingLineItem>() }
                }
            };

            foreach (var bill in overdueBills)
            {
                var daysOverdue = (reportDate - bill.DueDate).Days;
                var bucket = report.Buckets.FirstOrDefault(b => 
                    daysOverdue >= b.DaysFrom && (!b.DaysTo.HasValue || daysOverdue <= b.DaysTo.Value));

                if (bucket != null)
                {
                    bucket.Items.Add(new AgingLineItem
                    {
                        EntityId = bill.Id,
                        EntityNumber = bill.BillNumber,
                        CustomerOrVendorName = "Vendor", // Load from vendor repository
                        DueDate = bill.DueDate,
                        DaysOverdue = daysOverdue,
                        Amount = bill.TotalAmount
                    });
                }
            }

            // Calculate totals (same logic as AR aging)
            foreach (var bucket in report.Buckets)
            {
                bucket.SubTotal = new MonetaryValue(
                    bucket.Items.Sum(i => i.Amount.Amount),
                    input.Currency);
            }

            report.TotalOutstanding = new MonetaryValue(
                report.Buckets.Sum(b => b.SubTotal.Amount),
                input.Currency);

            foreach (var bucket in report.Buckets)
            {
                bucket.PercentageOfTotal = report.TotalOutstanding.Amount != 0
                    ? (bucket.SubTotal.Amount / report.TotalOutstanding.Amount) * 100
                    : 0;
            }

            return report;
        }

        public async Task<BudgetVarianceReportDto> GetBudgetVarianceReportAsync(GetReportInputDto input)
        {
            if (!input.BudgetId.HasValue)
            {
                throw new Volo.Abp.UserFriendlyException("Budget ID is required for variance report");
            }

            var budget = await _budgetRepository.GetWithAllocationsAsync(input.BudgetId.Value);

            var report = new BudgetVarianceReportDto
            {
                BudgetId = budget.Id,
                BudgetName = budget.Name,
                PeriodStart = budget.PeriodStart,
                PeriodEnd = budget.PeriodEnd,
                LineItems = new List<BudgetVarianceLineItem>()
            };

            foreach (var allocation in budget.Allocations)
            {
                var variance = allocation.ActualAmount.Amount - allocation.BudgetedAmount.Amount;
                var variancePercentage = allocation.BudgetedAmount.Amount != 0
                    ? (variance / allocation.BudgetedAmount.Amount) * 100
                    : 0;

                string status;
                if (Math.Abs(variancePercentage) <= 10)
                {
                    status = "On Track";
                }
                else if (variancePercentage > 10)
                {
                    status = "Over Budget";
                }
                else
                {
                    status = "Under Budget";
                }

                report.LineItems.Add(new BudgetVarianceLineItem
                {
                    CategoryId = allocation.CategoryId,
                    CategoryName = "Category", // Load from category service
                    Budgeted = allocation.BudgetedAmount,
                    Actual = allocation.ActualAmount,
                    Variance = new MonetaryValue(variance, allocation.BudgetedAmount.Currency),
                    VariancePercentage = variancePercentage,
                    Status = status
                });
            }

            report.TotalBudgeted = budget.TotalBudget;
            report.TotalActual = budget.ActualSpent;
            report.TotalVariance = new MonetaryValue(
                budget.ActualSpent.Amount - budget.TotalBudget.Amount,
                budget.TotalBudget.Currency);

            report.VariancePercentage = budget.TotalBudget.Amount != 0
                ? (report.TotalVariance.Amount / budget.TotalBudget.Amount) * 100
                : 0;

            return report;
        }
    }
}
