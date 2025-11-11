using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.Workflow;

[DependsOn(
    typeof(WorkflowDomainModule),
    typeof(WorkflowApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class WorkflowApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WorkflowApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WorkflowApplicationModule>(validate: true);
        });
    }
}
