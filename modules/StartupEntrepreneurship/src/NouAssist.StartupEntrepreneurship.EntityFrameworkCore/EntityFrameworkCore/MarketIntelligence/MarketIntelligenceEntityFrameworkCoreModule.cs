using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore
{
    [DependsOn(
        typeof(MarketIntelligenceDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class MarketIntelligenceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MarketIntelligenceDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Competitor, EfCoreCompetitorRepository>();
                options.AddRepository<MarketTrend, EfCoreMarketTrendRepository>();
                options.AddRepository<CustomerSegmentProfile, EfCoreCustomerSegmentProfileRepository>();
                options.AddRepository<SwotAnalysis, EfCoreSwotAnalysisRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
