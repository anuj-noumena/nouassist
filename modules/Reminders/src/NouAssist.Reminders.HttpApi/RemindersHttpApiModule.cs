using Localization.Resources.AbpUi;
using NouAssist.Reminders.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Reminders;

[DependsOn(
    typeof(RemindersApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class RemindersHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(RemindersHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<RemindersResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
