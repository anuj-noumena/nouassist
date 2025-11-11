using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.Workflow.EntityFrameworkCore;

[DependsOn(
    typeof(WorkflowDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class WorkflowEntityFrameworkCoreModule : AbpModule
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
