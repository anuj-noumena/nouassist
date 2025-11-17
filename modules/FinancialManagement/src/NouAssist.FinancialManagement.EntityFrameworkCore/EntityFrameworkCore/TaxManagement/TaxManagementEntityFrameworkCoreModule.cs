using Microsoft.Extensions.DependencyInjection;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.TaxManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.TaxManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(TaxManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class TaxManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TaxManagementDbContext>(options =>
            {
                options.AddRepository<TaxReturn, EfCoreTaxReturnRepository>();
                options.AddRepository<TaxProfile, EfCoreTaxProfileRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
