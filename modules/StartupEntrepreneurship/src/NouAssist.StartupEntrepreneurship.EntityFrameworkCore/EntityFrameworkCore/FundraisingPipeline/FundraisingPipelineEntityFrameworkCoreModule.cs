using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore
{
    [DependsOn(
        typeof(FundraisingPipelineDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class FundraisingPipelineEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<FundraisingPipelineDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Prospect, EfCoreProspectRepository>();
                options.AddRepository<FundraisingRound, EfCoreFundraisingRoundRepository>();
                options.AddRepository<PitchDeck, EfCorePitchDeckRepository>();
                options.AddRepository<TermSheetOffer, EfCoreTermSheetOfferRepository>();

                // Configure other DbContext options if needed
            });

            // Configure EF Core options to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
