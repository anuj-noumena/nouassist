using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore
{
    [DependsOn(
        typeof(PerformanceFinancialModelingDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class PerformanceFinancialModelingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<PerformanceFinancialModelingDbContext>(options =>
            {
                // Register CUSTOM repositories here for aggregate roots
                options.AddRepository<TrackedMetric, EfCoreTrackedMetricRepository>();
                options.AddRepository<FinancialModel, EfCoreFinancialModelRepository>();

                // Register default repositories for non-aggregate entities
                options.AddDefaultRepository<MetricDefinition>();
                options.AddDefaultRepository<MetricCategory>();

                // Configure context options if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
