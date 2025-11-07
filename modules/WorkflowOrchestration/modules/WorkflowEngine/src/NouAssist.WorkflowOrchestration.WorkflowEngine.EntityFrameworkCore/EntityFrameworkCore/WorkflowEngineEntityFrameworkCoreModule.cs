using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore;

[DependsOn(
    typeof(WorkflowEngineDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class WorkflowEngineEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WorkflowEngineDbContext>(options =>
        {
            options.AddDefaultRepositories<IWorkflowEngineDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
