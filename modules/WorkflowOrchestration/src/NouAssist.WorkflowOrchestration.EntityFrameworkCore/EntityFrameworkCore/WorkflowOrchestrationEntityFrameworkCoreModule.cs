using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.EntityFrameworkCore;

[DependsOn(
    typeof(WorkflowOrchestrationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class WorkflowOrchestrationEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WorkflowOrchestrationDbContext>(options =>
        {
            options.AddDefaultRepositories<IWorkflowOrchestrationDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
