using Microsoft.Extensions.DependencyInjection;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Workflow.EntityFrameworkCore;

[DependsOn(
    typeof(WorkflowDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSQLModule)
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

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
    }
}
