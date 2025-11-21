using System;
using System.Linq;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement
{
    // General Ledger Mappers
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LedgerAccountToLedgerAccountDtoMapper : MapperBase<LedgerAccount, LedgerAccountDto>
    {
        [MapperIgnoreTarget(nameof(LedgerAccountDto.AccountNumber))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.Currency))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.ParentAccountName))]
        public override partial LedgerAccountDto Map(LedgerAccount source);

        
        [MapperIgnoreTarget(nameof(LedgerAccountDto.AccountNumber))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.Currency))]
        [MapperIgnoreTarget(nameof(LedgerAccountDto.ParentAccountName))]
        public override partial void Map(LedgerAccount source, LedgerAccountDto destination);

        public override void AfterMap(LedgerAccount source, LedgerAccountDto destination)
        {
            destination.AccountNumber = source.AccountNumber.Number;
            destination.CurrentBalance = source.CurrentBalance.Amount;
            destination.Currency = source.CurrentBalance.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class JournalEntryBatchToJournalEntryBatchDtoMapper : MapperBase<JournalEntryBatch, JournalEntryBatchDto>
    {
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.PostingDate))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.IsBalanced))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.Lines))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.Currency))]
        public override partial JournalEntryBatchDto Map(JournalEntryBatch source);

        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.PostingDate))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.IsBalanced))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.Lines))]
        [MapperIgnoreTarget(nameof(JournalEntryBatchDto.Currency))]
        public override partial void Map(JournalEntryBatch source, JournalEntryBatchDto destination);

        public override void AfterMap(JournalEntryBatch source, JournalEntryBatchDto destination)
        {
            destination.PostingDate = source.PostingDate.Date;
            destination.IsBalanced = source.IsBalanced();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class JournalEntryLineToJournalEntryLineDtoMapper : MapperBase<JournalEntryLine, JournalEntryLineDto>
    {
        [MapperIgnoreSource(nameof(JournalEntryLine.DebitAmount))]
        [MapperIgnoreSource(nameof(JournalEntryLine.CreditAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.AccountName))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.DebitAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.CreditAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.Currency))]
        public override partial JournalEntryLineDto Map(JournalEntryLine source);

        [MapperIgnoreSource(nameof(JournalEntryLine.DebitAmount))]
        [MapperIgnoreSource(nameof(JournalEntryLine.CreditAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.AccountName))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.DebitAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.CreditAmount))]
        [MapperIgnoreTarget(nameof(JournalEntryLineDto.Currency))]
        public override partial void Map(JournalEntryLine source, JournalEntryLineDto destination);

        public override void AfterMap(JournalEntryLine source, JournalEntryLineDto destination)
        {
            destination.DebitAmount = source.DebitAmount != null ? source.DebitAmount.Amount : (decimal?)null;
            destination.CreditAmount = source.CreditAmount != null ? source.CreditAmount.Amount : (decimal?)null;
            destination.Currency = source.DebitAmount != null ? source.DebitAmount.Currency : source.CreditAmount.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AccountingPeriodToAccountingPeriodDtoMapper : MapperBase<AccountingPeriod, AccountingPeriodDto>
    {
        [MapperIgnoreSource(nameof(AccountingPeriod.TenantId))]
        [MapperIgnoreTarget(nameof(AccountingPeriodDto.ClosedByName))]
        public override partial AccountingPeriodDto Map(AccountingPeriod source);

        [MapperIgnoreSource(nameof(AccountingPeriod.TenantId))]
        [MapperIgnoreTarget(nameof(AccountingPeriodDto.ClosedByName))]
        public override partial void Map(AccountingPeriod source, AccountingPeriodDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BankStatementToBankStatementDtoMapper : MapperBase<BankStatement, BankStatementDto>
    {
        [MapperIgnoreSource(nameof(BankStatement.TenantId))]
        [MapperIgnoreSource(nameof(BankStatement.OpeningBalance))]
        [MapperIgnoreSource(nameof(BankStatement.ClosingBalance))]
        [MapperIgnoreSource(nameof(BankStatement.Lines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.OpeningBalance))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ClosingBalance))]
        [MapperIgnoreTarget(nameof(BankStatementDto.Currency))]
        [MapperIgnoreTarget(nameof(BankStatementDto.Lines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.TotalLines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ReconciledLines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ReconciliationPercentage))]
        public override partial BankStatementDto Map(BankStatement source);

        [MapperIgnoreSource(nameof(BankStatement.TenantId))]
        [MapperIgnoreSource(nameof(BankStatement.OpeningBalance))]
        [MapperIgnoreSource(nameof(BankStatement.ClosingBalance))]
        [MapperIgnoreSource(nameof(BankStatement.Lines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.OpeningBalance))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ClosingBalance))]
        [MapperIgnoreTarget(nameof(BankStatementDto.Currency))]
        [MapperIgnoreTarget(nameof(BankStatementDto.Lines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.TotalLines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ReconciledLines))]
        [MapperIgnoreTarget(nameof(BankStatementDto.ReconciliationPercentage))]
        public override partial void Map(BankStatement source, BankStatementDto destination);

        public override void AfterMap(BankStatement source, BankStatementDto destination)
        {
            destination.OpeningBalance = source.OpeningBalance.Amount;
            destination.ClosingBalance = source.ClosingBalance.Amount;
            destination.Currency = source.OpeningBalance.Currency;
            destination.TotalLines = source.Lines.Count;
            destination.ReconciledLines = source.Lines.Count(l => l.IsMatched);
            destination.ReconciliationPercentage = source.Lines.Any()
                ? (source.Lines.Count(l => l.IsMatched) * 100.0m / source.Lines.Count)
                : 0m;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BankStatementLineToBankStatementLineDtoMapper : MapperBase<BankStatementLine, BankStatementLineDto>
    {
        [MapperIgnoreSource(nameof(BankStatementLine.Amount))]
        [MapperIgnoreSource(nameof(BankStatementLine.ReferenceNumber))]
        [MapperIgnoreTarget(nameof(BankStatementLineDto.Reference))]
        [MapperIgnoreTarget(nameof(BankStatementLineDto.Amount))]
        public override partial BankStatementLineDto Map(BankStatementLine source);

        [MapperIgnoreSource(nameof(BankStatementLine.Amount))]
        [MapperIgnoreSource(nameof(BankStatementLine.ReferenceNumber))]
        [MapperIgnoreTarget(nameof(BankStatementLineDto.Reference))]
        [MapperIgnoreTarget(nameof(BankStatementLineDto.Amount))]
        public override partial void Map(BankStatementLine source, BankStatementLineDto destination);

        public override void AfterMap(BankStatementLine source, BankStatementLineDto destination)
        {
            destination.Reference = source.ReferenceNumber;
            destination.Amount = source.Amount.Amount;
        }
    }

    // Accounts Payable Mappers
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BillToBillDtoMapper : MapperBase<Bill, BillDto>
    {
        [MapperIgnoreSource(nameof(Bill.TenantId))]
        [MapperIgnoreSource(nameof(Bill.TotalAmount))]
        [MapperIgnoreSource(nameof(Bill.LineItems))]
        [MapperIgnoreTarget(nameof(BillDto.Amount))]
        [MapperIgnoreTarget(nameof(BillDto.Currency))]
        [MapperIgnoreTarget(nameof(BillDto.CategoryName))]
        [MapperIgnoreTarget(nameof(BillDto.ProjectName))]
        [MapperIgnoreTarget(nameof(BillDto.DaysUntilDue))]
        [MapperIgnoreTarget(nameof(BillDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(BillDto.LineItems))]
        public override partial BillDto Map(Bill source);

        [MapperIgnoreSource(nameof(Bill.TenantId))]
        [MapperIgnoreSource(nameof(Bill.TotalAmount))]
        [MapperIgnoreSource(nameof(Bill.LineItems))]
        [MapperIgnoreTarget(nameof(BillDto.Amount))]
        [MapperIgnoreTarget(nameof(BillDto.Currency))]
        [MapperIgnoreTarget(nameof(BillDto.CategoryName))]
        [MapperIgnoreTarget(nameof(BillDto.ProjectName))]
        [MapperIgnoreTarget(nameof(BillDto.DaysUntilDue))]
        [MapperIgnoreTarget(nameof(BillDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(BillDto.LineItems))]
        public override partial void Map(Bill source, BillDto destination);

        public override void AfterMap(Bill source, BillDto destination)
        {
            destination.Amount = source.TotalAmount.Amount;
            destination.Currency = source.TotalAmount.Currency;
            destination.DaysUntilDue = (source.DueDate - DateTime.UtcNow.Date).Days;
            destination.IsOverdue = source.IsOverdue();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BillApprovalToBillApprovalDtoMapper : MapperBase<BillApproval, BillApprovalDto>
    {
        [MapperIgnoreTarget(nameof(BillApprovalDto.ApproverName))]
        public override partial BillApprovalDto Map(BillApproval source);

        [MapperIgnoreTarget(nameof(BillApprovalDto.ApproverName))]
        public override partial void Map(BillApproval source, BillApprovalDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BillLineItemToBillLineItemDtoMapper : MapperBase<BillLineItem, BillLineItemDto>
    {
        [MapperIgnoreSource(nameof(BillLineItem.UnitPrice))]
        [MapperIgnoreSource(nameof(BillLineItem.LineTotal))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.UnitPrice))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.LineTotal))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.Currency))]
        public override partial BillLineItemDto Map(BillLineItem source);

        [MapperIgnoreSource(nameof(BillLineItem.UnitPrice))]
        [MapperIgnoreSource(nameof(BillLineItem.LineTotal))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.UnitPrice))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.LineTotal))]
        [MapperIgnoreTarget(nameof(BillLineItemDto.Currency))]
        public override partial void Map(BillLineItem source, BillLineItemDto destination);

        public override void AfterMap(BillLineItem source, BillLineItemDto destination)
        {
            destination.UnitPrice = source.UnitPrice.Amount;
            destination.LineTotal = source.LineTotal.Amount;
            destination.Currency = source.UnitPrice.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class VendorToVendorDtoMapper : MapperBase<Vendor, VendorDto>
    {
        [MapperIgnoreSource(nameof(Vendor.TenantId))]
        [MapperIgnoreSource(nameof(Vendor.CreditLimit))]
        [MapperIgnoreTarget(nameof(VendorDto.Name))]
        [MapperIgnoreTarget(nameof(VendorDto.Email))]
        [MapperIgnoreTarget(nameof(VendorDto.TaxId))]
        [MapperIgnoreTarget(nameof(VendorDto.CreditLimit))]
        [MapperIgnoreTarget(nameof(VendorDto.Currency))]
        public override partial VendorDto Map(Vendor source);

        [MapperIgnoreSource(nameof(Vendor.TenantId))]
        [MapperIgnoreSource(nameof(Vendor.CreditLimit))]
        [MapperIgnoreTarget(nameof(VendorDto.Name))]
        [MapperIgnoreTarget(nameof(VendorDto.Email))]
        [MapperIgnoreTarget(nameof(VendorDto.TaxId))]
        [MapperIgnoreTarget(nameof(VendorDto.CreditLimit))]
        [MapperIgnoreTarget(nameof(VendorDto.Currency))]
        public override partial void Map(Vendor source, VendorDto destination);

        public override void AfterMap(Vendor source, VendorDto destination)
        {
            destination.TaxId = source.TaxId.Value;
            destination.CreditLimit = source.CreditLimit.Amount;
            destination.Currency = source.CreditLimit.Currency;
        }
    }

    // Accounts Receivable Mappers
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InvoiceToInvoiceDtoMapper : MapperBase<Invoice, InvoiceDto>
    {
        [MapperIgnoreSource(nameof(Invoice.TenantId))]
        [MapperIgnoreSource(nameof(Invoice.TotalAmount))]
        [MapperIgnoreSource(nameof(Invoice.PaidAmount))]
        [MapperIgnoreSource(nameof(Invoice.LineItems))]
        [MapperIgnoreTarget(nameof(InvoiceDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(InvoiceDto.PaidAmount))]
        [MapperIgnoreTarget(nameof(InvoiceDto.RemainingBalance))]
        [MapperIgnoreTarget(nameof(InvoiceDto.Currency))]
        [MapperIgnoreTarget(nameof(InvoiceDto.DaysUntilDue))]
        [MapperIgnoreTarget(nameof(InvoiceDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(InvoiceDto.LineItems))]
        public override partial InvoiceDto Map(Invoice source);

        [MapperIgnoreSource(nameof(Invoice.TenantId))]
        [MapperIgnoreSource(nameof(Invoice.TotalAmount))]
        [MapperIgnoreSource(nameof(Invoice.PaidAmount))]
        [MapperIgnoreSource(nameof(Invoice.LineItems))]
        [MapperIgnoreTarget(nameof(InvoiceDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(InvoiceDto.PaidAmount))]
        [MapperIgnoreTarget(nameof(InvoiceDto.RemainingBalance))]
        [MapperIgnoreTarget(nameof(InvoiceDto.Currency))]
        [MapperIgnoreTarget(nameof(InvoiceDto.DaysUntilDue))]
        [MapperIgnoreTarget(nameof(InvoiceDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(InvoiceDto.LineItems))]
        public override partial void Map(Invoice source, InvoiceDto destination);

        public override void AfterMap(Invoice source, InvoiceDto destination)
        {
            destination.TotalAmount = source.TotalAmount.Amount;
            destination.PaidAmount = source.PaidAmount.Amount;
            destination.RemainingBalance = source.GetRemainingBalance().Amount;
            destination.Currency = source.TotalAmount.Currency;
            destination.DaysUntilDue = (source.DueDate - DateTime.UtcNow.Date).Days;
            destination.IsOverdue = source.IsOverdue();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InvoiceLineItemToInvoiceLineItemDtoMapper : MapperBase<InvoiceLineItem, InvoiceLineItemDto>
    {
        [MapperIgnoreSource(nameof(InvoiceLineItem.UnitPrice))]
        [MapperIgnoreSource(nameof(InvoiceLineItem.SubTotal))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.UnitPrice))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.SubTotal))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.Currency))]
        public override partial InvoiceLineItemDto Map(InvoiceLineItem source);

        [MapperIgnoreSource(nameof(InvoiceLineItem.UnitPrice))]
        [MapperIgnoreSource(nameof(InvoiceLineItem.SubTotal))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.UnitPrice))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.SubTotal))]
        [MapperIgnoreTarget(nameof(InvoiceLineItemDto.Currency))]
        public override partial void Map(InvoiceLineItem source, InvoiceLineItemDto destination);

        public override void AfterMap(InvoiceLineItem source, InvoiceLineItemDto destination)
        {
            destination.UnitPrice = source.UnitPrice.Amount;
            destination.SubTotal = source.SubTotal.Amount;
            destination.Currency = source.UnitPrice.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomerToCustomerDtoMapper : MapperBase<Customer, CustomerDto>
    {
        [MapperIgnoreSource(nameof(Customer.TenantId))]
        [MapperIgnoreSource(nameof(Customer.CreditLimit))]
        [MapperIgnoreSource(nameof(Customer.CurrentBalance))]
        [MapperIgnoreTarget(nameof(CustomerDto.Name))]
        [MapperIgnoreTarget(nameof(CustomerDto.Type))]
        [MapperIgnoreTarget(nameof(CustomerDto.TaxId))]
        [MapperIgnoreTarget(nameof(CustomerDto.CreditLimit))]
        [MapperIgnoreTarget(nameof(CustomerDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(CustomerDto.Currency))]
        public override partial CustomerDto Map(Customer source);

        [MapperIgnoreSource(nameof(Customer.TenantId))]
        [MapperIgnoreSource(nameof(Customer.CreditLimit))]
        [MapperIgnoreSource(nameof(Customer.CurrentBalance))]
        [MapperIgnoreTarget(nameof(CustomerDto.Name))]
        [MapperIgnoreTarget(nameof(CustomerDto.Type))]
        [MapperIgnoreTarget(nameof(CustomerDto.TaxId))]
        [MapperIgnoreTarget(nameof(CustomerDto.CreditLimit))]
        [MapperIgnoreTarget(nameof(CustomerDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(CustomerDto.Currency))]
        public override partial void Map(Customer source, CustomerDto destination);

        public override void AfterMap(Customer source, CustomerDto destination)
        {
            destination.TaxId = source.TaxId.Value;
            destination.CreditLimit = source.CreditLimit.Amount;
            destination.CurrentBalance = source.CurrentBalance.Amount;
            destination.Currency = source.CreditLimit.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomerPaymentToCustomerPaymentDtoMapper : MapperBase<CustomerPayment, CustomerPaymentDto>
    {
        [MapperIgnoreSource(nameof(CustomerPayment.TenantId))]
        [MapperIgnoreSource(nameof(CustomerPayment.Amount))]
        [MapperIgnoreTarget(nameof(CustomerPaymentDto.Amount))]
        [MapperIgnoreTarget(nameof(CustomerPaymentDto.Currency))]
        public override partial CustomerPaymentDto Map(CustomerPayment source);

        [MapperIgnoreSource(nameof(CustomerPayment.TenantId))]
        [MapperIgnoreSource(nameof(CustomerPayment.Amount))]
        [MapperIgnoreTarget(nameof(CustomerPaymentDto.Amount))]
        [MapperIgnoreTarget(nameof(CustomerPaymentDto.Currency))]
        public override partial void Map(CustomerPayment source, CustomerPaymentDto destination);

        public override void AfterMap(CustomerPayment source, CustomerPaymentDto destination)
        {
            destination.Amount = source.Amount.Amount;
            destination.Currency = source.Amount.Currency;
        }
    }

    // Financial Planning Mappers
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ExpenseToExpenseDtoMapper : MapperBase<Expense, ExpenseDto>
    {
        [MapperIgnoreSource(nameof(Expense.TenantId))]
        [MapperIgnoreSource(nameof(Expense.Amount))]
        [MapperIgnoreTarget(nameof(ExpenseDto.UserName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.CategoryName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.PayeeName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.Amount))]
        [MapperIgnoreTarget(nameof(ExpenseDto.Currency))]
        public override partial ExpenseDto Map(Expense source);

        [MapperIgnoreSource(nameof(Expense.TenantId))]
        [MapperIgnoreSource(nameof(Expense.Amount))]
        [MapperIgnoreTarget(nameof(ExpenseDto.UserName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.CategoryName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.PayeeName))]
        [MapperIgnoreTarget(nameof(ExpenseDto.Amount))]
        [MapperIgnoreTarget(nameof(ExpenseDto.Currency))]
        public override partial void Map(Expense source, ExpenseDto destination);

        public override void AfterMap(Expense source, ExpenseDto destination)
        {
            destination.Amount = source.Amount.Amount;
            destination.Currency = source.Amount.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BudgetToBudgetDtoMapper : MapperBase<Budget, BudgetDto>
    {
        [MapperIgnoreSource(nameof(Budget.TenantId))]
        [MapperIgnoreSource(nameof(Budget.TotalBudget))]
        [MapperIgnoreSource(nameof(Budget.ActualSpent))]
        [MapperIgnoreSource(nameof(Budget.Allocations))]
        [MapperIgnoreTarget(nameof(BudgetDto.TotalBudget))]
        [MapperIgnoreTarget(nameof(BudgetDto.ActualSpent))]
        [MapperIgnoreTarget(nameof(BudgetDto.Remaining))]
        [MapperIgnoreTarget(nameof(BudgetDto.VariancePercentage))]
        [MapperIgnoreTarget(nameof(BudgetDto.Currency))]
        [MapperIgnoreTarget(nameof(BudgetDto.Allocations))]
        public override partial BudgetDto Map(Budget source);

        [MapperIgnoreSource(nameof(Budget.TenantId))]
        [MapperIgnoreSource(nameof(Budget.TotalBudget))]
        [MapperIgnoreSource(nameof(Budget.ActualSpent))]
        [MapperIgnoreSource(nameof(Budget.Allocations))]
        [MapperIgnoreTarget(nameof(BudgetDto.TotalBudget))]
        [MapperIgnoreTarget(nameof(BudgetDto.ActualSpent))]
        [MapperIgnoreTarget(nameof(BudgetDto.Remaining))]
        [MapperIgnoreTarget(nameof(BudgetDto.VariancePercentage))]
        [MapperIgnoreTarget(nameof(BudgetDto.Currency))]
        [MapperIgnoreTarget(nameof(BudgetDto.Allocations))]
        public override partial void Map(Budget source, BudgetDto destination);

        public override void AfterMap(Budget source, BudgetDto destination)
        {
            destination.TotalBudget = source.TotalBudget.Amount;
            destination.ActualSpent = source.ActualSpent.Amount;
            destination.Remaining = source.TotalBudget.Amount - source.ActualSpent.Amount;
            destination.VariancePercentage = source.GetVariancePercentage();
            destination.Currency = source.TotalBudget.Currency;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CategoryAllocationToCategoryAllocationDtoMapper : MapperBase<CategoryAllocation, CategoryAllocationDto>
    {
        [MapperIgnoreSource(nameof(CategoryAllocation.BudgetedAmount))]
        [MapperIgnoreSource(nameof(CategoryAllocation.ActualAmount))]
        [MapperIgnoreSource(nameof(CategoryAllocation.Variance))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.BudgetedAmount))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.ActualAmount))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.Variance))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.PercentageUsed))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.IsOverBudget))]
        public override partial CategoryAllocationDto Map(CategoryAllocation source);

        [MapperIgnoreSource(nameof(CategoryAllocation.BudgetedAmount))]
        [MapperIgnoreSource(nameof(CategoryAllocation.ActualAmount))]
        [MapperIgnoreSource(nameof(CategoryAllocation.Variance))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.BudgetedAmount))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.ActualAmount))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.Variance))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.PercentageUsed))]
        [MapperIgnoreTarget(nameof(CategoryAllocationDto.IsOverBudget))]
        public override partial void Map(CategoryAllocation source, CategoryAllocationDto destination);

        public override void AfterMap(CategoryAllocation source, CategoryAllocationDto destination)
        {
            destination.BudgetedAmount = source.BudgetedAmount.Amount;
            destination.ActualAmount = source.ActualAmount.Amount;
            destination.Variance = source.Variance.Amount;
            destination.PercentageUsed = source.PercentageUsed;
            destination.IsOverBudget = source.IsOverBudget();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ExpenseReportToExpenseReportDtoMapper : MapperBase<ExpenseReport, ExpenseReportDto>
    {
        [MapperIgnoreSource(nameof(ExpenseReport.TenantId))]
        [MapperIgnoreSource(nameof(ExpenseReport.TotalAmount))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.Currency))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.ExpenseIds))]
        public override partial ExpenseReportDto Map(ExpenseReport source);

        [MapperIgnoreSource(nameof(ExpenseReport.TenantId))]
        [MapperIgnoreSource(nameof(ExpenseReport.TotalAmount))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.TotalAmount))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.Currency))]
        [MapperIgnoreTarget(nameof(ExpenseReportDto.ExpenseIds))]
        public override partial void Map(ExpenseReport source, ExpenseReportDto destination);

        public override void AfterMap(ExpenseReport source, ExpenseReportDto destination)
        {
            destination.TotalAmount = source.TotalAmount.Amount;
            destination.Currency = source.TotalAmount.Currency;
            destination.ExpenseIds = source.ExpenseIds.ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FinancialGoalToFinancialGoalDtoMapper : MapperBase<FinancialGoal, FinancialGoalDto>
    {
        [MapperIgnoreSource(nameof(FinancialGoal.TenantId))]
        [MapperIgnoreSource(nameof(FinancialGoal.TargetAmount))]
        [MapperIgnoreSource(nameof(FinancialGoal.CurrentAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.TargetAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.CurrentAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.Currency))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.ProgressPercentage))]
        public override partial FinancialGoalDto Map(FinancialGoal source);

        [MapperIgnoreSource(nameof(FinancialGoal.TenantId))]
        [MapperIgnoreSource(nameof(FinancialGoal.TargetAmount))]
        [MapperIgnoreSource(nameof(FinancialGoal.CurrentAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.TargetAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.CurrentAmount))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.Currency))]
        [MapperIgnoreTarget(nameof(FinancialGoalDto.ProgressPercentage))]
        public override partial void Map(FinancialGoal source, FinancialGoalDto destination);

        public override void AfterMap(FinancialGoal source, FinancialGoalDto destination)
        {
            destination.TargetAmount = source.TargetAmount.Amount;
            destination.CurrentAmount = source.CurrentAmount.Amount;
            destination.Currency = source.TargetAmount.Currency;
            destination.ProgressPercentage = source.GetProgressPercentage();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LinkedBankAccountToLinkedBankAccountDtoMapper : MapperBase<LinkedBankAccount, LinkedBankAccountDto>
    {
        [MapperIgnoreSource(nameof(LinkedBankAccount.TenantId))]
        [MapperIgnoreSource(nameof(LinkedBankAccount.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.Currency))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.AccountName))]
        public override partial LinkedBankAccountDto Map(LinkedBankAccount source);

        [MapperIgnoreSource(nameof(LinkedBankAccount.TenantId))]
        [MapperIgnoreSource(nameof(LinkedBankAccount.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.CurrentBalance))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.Currency))]
        [MapperIgnoreTarget(nameof(LinkedBankAccountDto.AccountName))]
        public override partial void Map(LinkedBankAccount source, LinkedBankAccountDto destination);

        public override void AfterMap(LinkedBankAccount source, LinkedBankAccountDto destination)
        {
            destination.CurrentBalance = source.CurrentBalance.Amount;
            destination.Currency = source.CurrentBalance.Currency;
            destination.AccountName = source.AccountName;
        }
    }
}
