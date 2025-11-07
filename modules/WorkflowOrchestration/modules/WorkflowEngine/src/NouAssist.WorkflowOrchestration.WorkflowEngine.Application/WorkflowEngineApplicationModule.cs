using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(WorkflowEngineDomainModule),
    typeof(WorkflowEngineApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class WorkflowEngineApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WorkflowEngineApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WorkflowEngineApplicationModule>(validate: true);
        });
    }
}
