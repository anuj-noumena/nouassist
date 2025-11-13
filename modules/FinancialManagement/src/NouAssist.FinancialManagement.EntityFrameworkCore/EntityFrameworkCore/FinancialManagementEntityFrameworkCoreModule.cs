using Microsoft.Extensions.DependencyInjection;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsPayable;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsReceivable;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.FinancialPlanning;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.GeneralLedger;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(FinancialManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class FinancialManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FinancialManagementDbContext>(options =>
            {
                // General Ledger Repositories
                options.AddRepository<LedgerAccount, EfCoreLedgerAccountRepository>();
                options.AddRepository<JournalEntryBatch, EfCoreJournalEntryRepository>();
                options.AddRepository<AccountingPeriod, EfCoreAccountingPeriodRepository>();

                // Accounts Payable Repositories
                options.AddRepository<Vendor, EfCoreVendorRepository>();
                options.AddRepository<Bill, EfCoreBillRepository>();

                // Accounts Receivable Repositories
                options.AddRepository<Customer, EfCoreCustomerRepository>();
                options.AddRepository<Invoice, EfCoreInvoiceRepository>();
                options.AddDefaultRepository<CustomerPayment>();
                options.AddDefaultRepository<CreditMemo>();
                options.AddDefaultRepository<CollectionCase>();

                // Financial Planning Repositories
                options.AddRepository<Expense, EfCoreExpenseRepository>();
                options.AddRepository<Budget, EfCoreBudgetRepository>();
                options.AddRepository<FinancialGoal, EfCoreFinancialGoalRepository>();

                // Enable default repositories for other aggregates
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<FinancialManagementDbContext>(ctx =>
                {
                    ctx.UseNpgsql();
                });
            });
        }
    }
}
