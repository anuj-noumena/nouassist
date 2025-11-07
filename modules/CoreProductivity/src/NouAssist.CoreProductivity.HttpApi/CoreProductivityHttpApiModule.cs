using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(CoreProductivityApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CoreProductivityHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CoreProductivityHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CoreProductivityResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
