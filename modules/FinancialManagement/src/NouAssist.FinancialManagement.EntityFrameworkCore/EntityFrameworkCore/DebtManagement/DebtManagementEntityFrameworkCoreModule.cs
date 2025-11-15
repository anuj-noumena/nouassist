using Microsoft.Extensions.DependencyInjection;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.Data;
using NouAssist.DebtManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.DebtManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(DebtManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class DebtManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DebtManagementDbContext>(options =>
            {
                options.AddRepository<DebtAccount, EfCoreDebtAccountRepository>();
                options.AddRepository<DebtPayoffPlan, EfCoreDebtPayoffPlanRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
