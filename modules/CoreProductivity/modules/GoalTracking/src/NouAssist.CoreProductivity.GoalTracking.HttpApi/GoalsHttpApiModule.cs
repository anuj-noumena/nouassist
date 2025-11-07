using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity.GoalTracking;

[DependsOn(
    typeof(GoalTrackingApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class GoalTrackingHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GoalTrackingHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<GoalTrackingResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
