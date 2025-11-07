using Localization.Resources.AbpUi;
using NouAssist.Goals.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Goals;

[DependsOn(
    typeof(GoalsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class GoalsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GoalsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<GoalsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
