using Localization.Resources.AbpUi;
using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ActivityTrackingHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ActivityTrackingHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ActivityTrackingResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
