using Microsoft.Extensions.DependencyInjection;
using NouAssist.InvestmentManagement.Aggregates;
using NouAssist.InvestmentManagement.Data;
using NouAssist.InvestmentManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(InvestmentManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class InvestmentManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<InvestmentManagementDbContext>(options =>
            {
                options.AddRepository<Portfolio, EfCorePortfolioRepository>();
                options.AddRepository<InvestmentGoal, EfCoreInvestmentGoalRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
