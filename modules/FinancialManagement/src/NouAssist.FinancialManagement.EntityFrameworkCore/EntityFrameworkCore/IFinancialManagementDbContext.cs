using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.Shared;
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
        DbSet<Bill> Bills { get; }
        DbSet<Vendor> Vendors { get; }

        // Accounts Receivable
        DbSet<Invoice> Invoices { get; }
        DbSet<Customer> Customers { get; }
        DbSet<CustomerPayment> CustomerPayments { get; }

        // Financial Planning & Analysis
        DbSet<Expense> Expenses { get; }
        DbSet<ExpenseReport> ExpenseReports { get; }
        DbSet<Budget> Budgets { get; }
        DbSet<FinancialGoal> FinancialGoals { get; }
        DbSet<LinkedBankAccount> LinkedBankAccounts { get; }
    }
}
