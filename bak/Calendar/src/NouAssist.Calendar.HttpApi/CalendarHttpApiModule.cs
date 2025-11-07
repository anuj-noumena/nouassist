using Localization.Resources.AbpUi;
using NouAssist.Calendar.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Calendar;

[DependsOn(
    typeof(CalendarApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CalendarHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CalendarHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CalendarResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
