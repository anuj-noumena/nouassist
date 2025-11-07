using Localization.Resources.AbpUi;
using NouAssist.Habits.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Habits;

[DependsOn(
    typeof(HabitsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class HabitsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(HabitsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<HabitsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
