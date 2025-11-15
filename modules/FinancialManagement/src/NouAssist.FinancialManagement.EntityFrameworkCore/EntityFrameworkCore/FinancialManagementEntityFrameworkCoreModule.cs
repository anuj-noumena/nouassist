using Microsoft.Extensions.DependencyInjection;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsPayable;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsReceivable;
using NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.GeneralLedger;
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
                // Register custom repositories
                options.AddRepository<LedgerAccount, EfCoreLedgerAccountRepository>();
                options.AddRepository<JournalEntryBatch, EfCoreJournalEntryBatchRepository>();
                options.AddRepository<AccountingPeriod, EfCoreAccountingPeriodRepository>();
                
                options.AddRepository<Bill, EfCoreBillRepository>();
                options.AddRepository<Vendor, EfCoreRepository<IFinancialManagementDbContext, Vendor, Guid>>();
                
                options.AddRepository<Invoice, EfCoreInvoiceRepository>();
                options.AddRepository<Customer, EfCoreRepository<IFinancialManagementDbContext, Customer, Guid>>();

                // Add default repositories for other aggregates
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
