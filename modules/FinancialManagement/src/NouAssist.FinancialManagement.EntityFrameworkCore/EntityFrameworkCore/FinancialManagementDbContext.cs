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
    public class FinancialManagementDbContext 
        : AbpDbContext<FinancialManagementDbContext>, IFinancialManagementDbContext
    {
        // General Ledger
        public DbSet<LedgerAccount> LedgerAccounts { get; set; }
        public DbSet<JournalEntryBatch> JournalEntryBatches { get; set; }
        public DbSet<AccountingPeriod> AccountingPeriods { get; set; }
        public DbSet<BankStatement> BankStatements { get; set; }

        // Accounts Payable
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        // Accounts Receivable
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }

        // Financial Planning & Analysis
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<FinancialGoal> FinancialGoals { get; set; }
        public DbSet<LinkedBankAccount> LinkedBankAccounts { get; set; }

        public FinancialManagementDbContext(
            DbContextOptions<FinancialManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFinancialManagement(this);
        }
    }
}
