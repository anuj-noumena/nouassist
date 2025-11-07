using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore;

[DependsOn(
    typeof(ApprovalWorkflowsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ApprovalWorkflowsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ApprovalDbContext>(options =>
        {
            options.AddDefaultRepositories<IApprovalDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
