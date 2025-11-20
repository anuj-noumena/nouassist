using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    [DependsOn(
        typeof(RequirementsAnalysisToolsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class RequirementsAnalysisToolsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RequirementsAnalysisToolsDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<AnalyzedRequirement, EfCoreAnalyzedRequirementRepository>();
                options.AddRepository<GlossaryTerm, EfCoreGlossaryTermRepository>();
                options.AddRepository<PrioritizationActivity, EfCorePrioritizationActivityRepository>();

                // Add default repositories for aggregates if needed
                // This will automatically create repositories for entities that don't have custom implementations
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options for PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
