using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(WorkflowOrchestrationDomainModule),
    typeof(WorkflowOrchestrationApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class WorkflowOrchestrationApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WorkflowOrchestrationApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WorkflowOrchestrationApplicationModule>(validate: true);
        });
    }
}
