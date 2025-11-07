using Localization.Resources.AbpUi;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(WorkflowEngineApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class WorkflowEngineHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(WorkflowEngineHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WorkflowEngineResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
