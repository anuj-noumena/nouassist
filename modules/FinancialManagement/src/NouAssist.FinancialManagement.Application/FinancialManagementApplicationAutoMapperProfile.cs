using AutoMapper;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Application.Contracts.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.Application.Contracts.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.Application.Contracts.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Entities;

namespace NouAssist.FinancialManagement.Application
{
    public class FinancialManagementApplicationAutoMapperProfile : Profile
    {
        public FinancialManagementApplicationAutoMapperProfile()
        {
            // General Ledger Mappings
            CreateMap<LedgerAccount, LedgerAccountDto>()
                .ForMember(d => d.AccountNumber, opt => opt.MapFrom(s => s.AccountNumber.Number))
                .ForMember(d => d.CurrentBalance, opt => opt.MapFrom(s => s.CurrentBalance.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.CurrentBalance.Currency));

            CreateMap<JournalEntryBatch, JournalEntryBatchDto>()
                .ForMember(d => d.PostingDate, opt => opt.MapFrom(s => s.PostingDate.Date))
                .ForMember(d => d.IsBalanced, opt => opt.MapFrom(s => s.IsBalanced()));

            CreateMap<JournalEntryLine, JournalEntryLineDto>()
                .ForMember(d => d.DebitAmount, opt => opt.MapFrom(s => s.DebitAmount != null ? s.DebitAmount.Amount : (decimal?)null))
                .ForMember(d => d.CreditAmount, opt => opt.MapFrom(s => s.CreditAmount != null ? s.CreditAmount.Amount : (decimal?)null))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => 
                    s.DebitAmount != null ? s.DebitAmount.Currency : s.CreditAmount.Currency));

            CreateMap<AccountingPeriod, AccountingPeriodDto>();

            // Accounts Payable Mappings
            CreateMap<Bill, BillDto>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Amount.Currency))
                .ForMember(d => d.DaysUntilDue, opt => opt.MapFrom(s => (s.DueDate - DateTime.UtcNow.Date).Days))
                .ForMember(d => d.IsOverdue, opt => opt.MapFrom(s => s.IsOverdue()));

            CreateMap<BillApproval, BillApprovalDto>();

            CreateMap<BillLineItem, BillLineItemDto>()
                .ForMember(d => d.UnitPrice, opt => opt.MapFrom(s => s.UnitPrice.Amount))
                .ForMember(d => d.LineTotal, opt => opt.MapFrom(s => s.LineTotal.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.UnitPrice.Currency));

            CreateMap<Vendor, VendorDto>()
                .ForMember(d => d.TaxId, opt => opt.MapFrom(s => s.TaxId.Value))
                .ForMember(d => d.CreditLimit, opt => opt.MapFrom(s => s.CreditLimit.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.CreditLimit.Currency));

            // Accounts Receivable Mappings
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(d => d.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount.Amount))
                .ForMember(d => d.PaidAmount, opt => opt.MapFrom(s => s.PaidAmount.Amount))
                .ForMember(d => d.RemainingBalance, opt => opt.MapFrom(s => s.GetRemainingBalance().Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.TotalAmount.Currency))
                .ForMember(d => d.DaysUntilDue, opt => opt.MapFrom(s => (s.DueDate - DateTime.UtcNow.Date).Days))
                .ForMember(d => d.IsOverdue, opt => opt.MapFrom(s => s.IsOverdue()));

            CreateMap<InvoiceLineItem, InvoiceLineItemDto>()
                .ForMember(d => d.UnitPrice, opt => opt.MapFrom(s => s.UnitPrice.Amount))
                .ForMember(d => d.SubTotal, opt => opt.MapFrom(s => s.SubTotal.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.UnitPrice.Currency));

            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.TaxId, opt => opt.MapFrom(s => s.TaxId.Value))
                .ForMember(d => d.CreditLimit, opt => opt.MapFrom(s => s.CreditLimit.Amount))
                .ForMember(d => d.CurrentBalance, opt => opt.MapFrom(s => s.CurrentBalance.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.CreditLimit.Currency));

            // Financial Planning Mappings
            CreateMap<Expense, ExpenseDto>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Amount.Currency));

            CreateMap<Budget, BudgetDto>()
                .ForMember(d => d.TotalBudget, opt => opt.MapFrom(s => s.TotalBudget.Amount))
                .ForMember(d => d.ActualSpent, opt => opt.MapFrom(s => s.ActualSpent.Amount))
                .ForMember(d => d.Remaining, opt => opt.MapFrom(s => (s.TotalBudget.Amount - s.ActualSpent.Amount)))
                .ForMember(d => d.VariancePercentage, opt => opt.MapFrom(s => s.GetVariancePercentage()))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.TotalBudget.Currency));

            CreateMap<CategoryAllocation, CategoryAllocationDto>()
                .ForMember(d => d.BudgetedAmount, opt => opt.MapFrom(s => s.BudgetedAmount.Amount))
                .ForMember(d => d.ActualAmount, opt => opt.MapFrom(s => s.ActualAmount.Amount))
                .ForMember(d => d.Variance, opt => opt.MapFrom(s => s.Variance.Amount))
                .ForMember(d => d.PercentageUsed, opt => opt.MapFrom(s => s.PercentageUsed))
                .ForMember(d => d.IsOverBudget, opt => opt.MapFrom(s => s.IsOverBudget()));
        }
    }
}