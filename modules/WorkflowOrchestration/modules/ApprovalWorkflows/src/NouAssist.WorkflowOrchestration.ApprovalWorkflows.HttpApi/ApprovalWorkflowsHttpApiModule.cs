using Localization.Resources.AbpUi;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

[DependsOn(
    typeof(ApprovalWorkflowsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ApprovalWorkflowsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ApprovalWorkflowsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ApprovalWorkflowsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
