using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Data;
using NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore
{
    [DependsOn(
        typeof(GtdDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class GtdEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GtdDbContext>(options =>
            {
                options.AddRepository<GtdHorizon, EfCoreGtdHorizonRepository>();
                options.AddRepository<GtdHorizonAlignment, EfCoreGtdHorizonAlignmentRepository>();
                options.AddRepository<SomedayMaybeItem, EfCoreSomedayMaybeRepository>();
                options.AddRepository<TicklerItem, EfCoreTicklerRepository>();
                options.AddRepository<WaitingFor, EfCoreWaitingForRepository>();
                options.AddRepository<Followup, EfCoreFollowupRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
