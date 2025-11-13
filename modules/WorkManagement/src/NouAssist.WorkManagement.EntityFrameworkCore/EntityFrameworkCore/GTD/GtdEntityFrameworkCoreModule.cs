using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
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
