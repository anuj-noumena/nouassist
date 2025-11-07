using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity.TimeManagement;

[DependsOn(
    typeof(TimeManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class TimeManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(TimeManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TimeManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
