using Microsoft.Extensions.DependencyInjection;
using NouAssist.Analytics.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Analytics.EntityFrameworkCore
{
    [DependsOn(
        typeof(AnalyticsDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AnalyticsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AnalyticsDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: false);
                
                // Register custom repositories
                options.AddRepository<Dashboard, DashboardRepository>();
                options.AddRepository<ReportConfiguration, ReportConfigurationRepository>();
                options.AddRepository<KpiDefinition, KpiDefinitionRepository>();
                options.AddRepository<DataSource, DataSourceRepository>();
            });
            
            Configure<AbpDbContextOptions>(options =>
            {
                // Configure for PostgreSQL
                options.UseNpgsql();
            });
        }
    }
}
