using Microsoft.Extensions.DependencyInjection;
using NouAssist.CreditScoreMonitoring.Aggregates;
using NouAssist.CreditScoreMonitoring.Data;
using NouAssist.CreditScoreMonitoring.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore
{
    [DependsOn(
        typeof(CreditScoreMonitoringDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class CreditScoreMonitoringEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CreditScoreMonitoringDbContext>(options =>
            {
                options.AddRepository<CreditReport, EfCoreCreditReportRepository>();
                options.AddRepository<CreditDispute, EfCoreCreditDisputeRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
