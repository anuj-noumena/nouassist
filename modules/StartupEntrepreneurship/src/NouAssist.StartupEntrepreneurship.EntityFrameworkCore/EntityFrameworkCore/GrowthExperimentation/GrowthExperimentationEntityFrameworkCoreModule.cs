using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore
{
    [DependsOn(
        typeof(GrowthExperimentationDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class GrowthExperimentationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<GrowthExperimentationDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<GrowthExperiment, EfCoreGrowthExperimentRepository>();
                options.AddRepository<MarketingChannelPerformance, EfCoreMarketingChannelPerformanceRepository>();
                options.AddRepository<ConversionFunnelDefinition, EfCoreConversionFunnelDefinitionRepository>();
            });

            // Configure EF Core to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
