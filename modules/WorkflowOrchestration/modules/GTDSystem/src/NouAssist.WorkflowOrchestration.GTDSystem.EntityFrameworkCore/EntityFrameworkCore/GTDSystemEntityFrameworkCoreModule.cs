using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore;

[DependsOn(
    typeof(GTDSystemDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class GTDSystemEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GTDSystemDbContext>(options =>
        {
            options.AddDefaultRepositories<IGTDSystemDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
