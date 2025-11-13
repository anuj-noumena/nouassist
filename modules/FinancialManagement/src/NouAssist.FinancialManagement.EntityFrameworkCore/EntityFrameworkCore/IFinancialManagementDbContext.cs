using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    [ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
    public interface IFinancialManagementDbContext : IEfCoreDbContext
    {
        // General Ledger
        DbSet<LedgerAccount> LedgerAccounts { get; }
        DbSet<JournalEntryBatch> JournalEntryBatches { get; }
        DbSet<AccountingPeriod> AccountingPeriods { get; }
        DbSet<BankStatement> BankStatements { get; }

        // Accounts Payable
        DbSet<Vendor> Vendors { get; }
        DbSet<Bill> Bills { get; }

        // Accounts Receivable
        DbSet<Customer> Customers { get; }
        DbSet<Invoice> Invoices { get; }
        DbSet<CustomerPayment> CustomerPayments { get; }
        DbSet<CreditMemo> CreditMemos { get; }
        DbSet<CollectionCase> CollectionCases { get; }

        // Financial Planning & Analysis
        DbSet<Expense> Expenses { get; }
        DbSet<Budget> Budgets { get; }
        DbSet<FinancialGoal> FinancialGoals { get; }
    }
}
