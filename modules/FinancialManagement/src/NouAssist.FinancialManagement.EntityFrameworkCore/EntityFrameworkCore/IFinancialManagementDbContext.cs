using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    [ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
    public interface IFinancialManagementDbContext : IEfCoreDbContext
    {        
        DbSet<AuditEntry> AuditEntries { get; }
        
        // credit
        DbSet<CreditReport> CreditReports { get; }
        DbSet<CreditDispute> CreditDisputes { get; }
        
        // Currency
        DbSet<Currency> Currencies { get; }
        DbSet<CurrencyExchangeRate> ExchangeRates { get; }
        
        // Debt
        DbSet<DebtAccount> DebtAccounts { get; }
        DbSet<DebtPayoffPlan> DebtPayoffPlans { get; }
        
        // Document
        DbSet<Document> Documents { get; }
        DbSet<DocumentFolder> DocumentFolders { get; }
        
        // Investment
        DbSet<Portfolio> Portfolios { get; }
        DbSet<InvestmentGoal> InvestmentGoals { get; }
        
        // Tax Management
        DbSet<TaxReturn> TaxReturns { get; }
        DbSet<TaxProfile> TaxProfiles { get; }
        
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
