using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(HabitSystemsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class HabitSystemsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(HabitSystemsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<HabitSystemsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
