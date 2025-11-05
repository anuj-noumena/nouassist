using Localization.Resources.AbpUi;
using NouAssist.Activity.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Activity;

[DependsOn(
    typeof(ActivityApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ActivityHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ActivityHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ActivityResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
