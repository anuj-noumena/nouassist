using Microsoft.Extensions.DependencyInjection;
using NouAssist.CurrencyManagement.EntityFrameworkCore.Repositories;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(CurrencyManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class CurrencyManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CurrencyManagementDbContext>(options =>
            {
                options.AddRepository<Currency, EfCoreCurrencyRepository>();
                options.AddRepository<CurrencyExchangeRate, EfCoreCurrencyExchangeRateRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
