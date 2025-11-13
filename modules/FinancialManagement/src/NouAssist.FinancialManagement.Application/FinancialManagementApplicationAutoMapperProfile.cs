using AutoMapper;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Entities;

namespace NouAssist.FinancialManagement
{
    public class FinancialManagementApplicationAutoMapperProfile : Profile
    {
        public FinancialManagementApplicationAutoMapperProfile()
        {
            // General Ledger Mappings
            CreateMap<LedgerAccount, LedgerAccountDto>();
            CreateMap<JournalEntryBatch, JournalEntryBatchDto>();
            CreateMap<JournalEntryLine, JournalEntryLineDto>();

            // Accounts Payable Mappings
            CreateMap<Vendor, VendorDto>();
            CreateMap<Bill, BillDto>()
                .ForMember(dest => dest.VendorName, opt => opt.Ignore()); // Will be loaded separately
            CreateMap<AccountsPayable.Entities.BillLineItem, BillLineItemDto>();
            CreateMap<AccountsPayable.Entities.BillApproval, BillApprovalDto>();

            // Accounts Receivable Mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.Ignore());
            CreateMap<AccountsReceivable.Entities.InvoiceLineItem, InvoiceLineItemDto>();
            CreateMap<CustomerPayment, CustomerPaymentDto>();

            // Financial Planning Mappings
            CreateMap<Expense, ExpenseDto>();
            CreateMap<Budget, BudgetDto>();
            CreateMap<FinancialPlanning.Entities.CategoryAllocation, CategoryAllocationDto>();
            CreateMap<FinancialGoal, FinancialGoalDto>();
        }
    }
}
