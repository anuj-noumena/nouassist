using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkManagement.EntityFrameworkCore.Workflow.Repositories;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow;

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
            options.AddRepository<WorkManagement.Workflow.Aggregates.Workflow, EfCoreWorkflowRepository>();
            options.AddRepository<WorkflowScheme, EfCoreWorkflowSchemeRepository>();
            options.AddRepository<WorkflowCustomField, EfCoreWorkflowCustomFieldRepository>();

            options.AddDefaultRepositories<IWorkflowDbContext>(includeAllEntities: true);
        });
    }
}
