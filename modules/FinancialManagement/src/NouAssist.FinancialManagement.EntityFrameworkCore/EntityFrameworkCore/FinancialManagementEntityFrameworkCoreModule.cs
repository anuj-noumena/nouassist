using Microsoft.Extensions.DependencyInjection;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.AccountsReceivable.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.AuditTrail.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.CreditScoreMonitoring.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.CurrencyManagement.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.DebtManagement.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.DocumentManagement.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.InvestmentManagement.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.TaxManagement.Repositories;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.EntityFrameworkCore;

[DependsOn(
    typeof(FinancialManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FinancialManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FinancialManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IFinancialManagementDbContext>(includeAllEntities: true);

            // Audit Trail repositories
            options.AddRepository<AuditEntry, EfCoreAuditEntryRepository>();

            // Credit Score Monitoring repositories
            options.AddRepository<CreditReport, EfCoreCreditReportRepository>();
            options.AddRepository<CreditDispute, EfCoreCreditDisputeRepository>();

            // Currency Management repositories
            options.AddRepository<Currency, EfCoreCurrencyRepository>();
            options.AddRepository<CurrencyExchangeRate, EfCoreCurrencyExchangeRateRepository>();

            // Debt Management repositories
            options.AddRepository<DebtAccount, EfCoreDebtAccountRepository>();
            options.AddRepository<DebtPayoffPlan, EfCoreDebtPayoffPlanRepository>();

            // Document Management repositories
            options.AddRepository<Document, EfCoreDocumentRepository>();
            options.AddRepository<DocumentFolder, EfCoreDocumentFolderRepository>();

            // General Ledger repositories
            options.AddRepository<LedgerAccount, EfCoreLedgerAccountRepository>();
            options.AddRepository<JournalEntryBatch, EfCoreJournalEntryBatchRepository>();
            options.AddRepository<AccountingPeriod, EfCoreAccountingPeriodRepository>();

            // Investment Management repositories
            options.AddRepository<Portfolio, EfCorePortfolioRepository>();
            options.AddRepository<InvestmentGoal, EfCoreInvestmentGoalRepository>();

            // Tax Management repositories
            options.AddRepository<TaxReturn, EfCoreTaxReturnRepository>();
            options.AddRepository<TaxProfile, EfCoreTaxProfileRepository>();

            // Accounts Payable repositories
            options.AddRepository<Bill, EfCoreBillRepository>();
            options.AddRepository<Vendor, EfCoreVendorRepository>();

            // Accounts Receivable repositories
            options.AddRepository<Invoice, EfCoreInvoiceRepository>();
            options.AddRepository<Customer, EfCoreCustomerRepository>();
            options.AddRepository<CustomerPayment, EfCoreCustomerPaymentRepository>();

            // Financial Planning repositories
            options.AddRepository<Budget, EfCoreBudgetRepository>();
            options.AddRepository<Expense, EfCoreExpenseRepository>();
            options.AddRepository<ExpenseReport, EfCoreExpenseReportRepository>();
            options.AddRepository<FinancialGoal, EfCoreFinancialGoalRepository>();
            options.AddRepository<LinkedBankAccount, EfCoreLinkedBankAccountRepository>();
        });
    }
}