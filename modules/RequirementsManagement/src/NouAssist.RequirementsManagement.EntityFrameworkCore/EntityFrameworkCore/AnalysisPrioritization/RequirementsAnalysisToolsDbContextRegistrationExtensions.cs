using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    public static class RequirementsAnalysisToolsDbContextRegistrationExtensions
    {
        public static IServiceCollection AddRequirementsAnalysisToolsDbContext(
            this IServiceCollection services,
            Action<AbpDbContextRegistrationOptions> optionsAction = null)
        {
            services.AddAbpDbContext<RequirementsAnalysisToolsDbContext>(options =>
            {
                options.AddRepository<AnalyzedRequirement, EfCoreAnalyzedRequirementRepository>();
                options.AddRepository<GlossaryTerm, EfCoreGlossaryTermRepository>();
                options.AddRepository<PrioritizationActivity, EfCorePrioritizationActivityRepository>();

                optionsAction?.Invoke(options);
            });

            return services;
        }
    }
}
