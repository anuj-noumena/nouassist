using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore;

[DependsOn(
    typeof(LearningDevelopmentDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class LearningDevelopmentEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<LearningDevelopmentDbContext>(options =>
        {
            options.AddDefaultRepositories<ILearningDevelopmentDbContext>(includeAllEntities: true);

            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */

            // Integration repositories
            options.AddRepository<LearningDevelopment.Integration.Aggregates.LearningPath,
                Integration.Repositories.EfCoreLearningPathRepository>();
            options.AddRepository<LearningDevelopment.Integration.Aggregates.LearningActivity,
                Integration.Repositories.EfCoreLearningActivityRepository>();
        });
    }
}
