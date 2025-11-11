using Localization.Resources.AbpUi;
using NouAssist.Customization.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Customization;

[DependsOn(
    typeof(CustomizationApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CustomizationHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CustomizationHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CustomizationResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
