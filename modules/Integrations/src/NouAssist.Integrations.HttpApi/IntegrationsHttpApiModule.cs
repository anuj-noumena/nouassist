using Localization.Resources.AbpUi;
using NouAssist.Integrations.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(IntegrationsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class IntegrationsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(IntegrationsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IntegrationsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
