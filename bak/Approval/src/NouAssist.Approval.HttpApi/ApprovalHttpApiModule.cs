using Localization.Resources.AbpUi;
using NouAssist.Approval.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Approval;

[DependsOn(
    typeof(ApprovalApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ApprovalHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ApprovalHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ApprovalResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
