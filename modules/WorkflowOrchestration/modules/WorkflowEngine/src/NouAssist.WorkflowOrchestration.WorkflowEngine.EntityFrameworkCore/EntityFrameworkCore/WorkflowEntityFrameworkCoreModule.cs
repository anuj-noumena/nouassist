using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Repositories;
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
        context.Services.AddAbpDbContext<WorkflowDbContext>(options =>
        {
            options.AddRepository<Aggregates.Workflow, EfCoreWorkflowRepository>();
            options.AddRepository<WorkflowScheme, EfCoreWorkflowSchemeRepository>();
            options.AddRepository<WorkflowCustomField, EfCoreWorkflowCustomFieldRepository>();

            options.AddDefaultRepositories<IWorkflowDbContext>(includeAllEntities: true);
        });
    }
}
