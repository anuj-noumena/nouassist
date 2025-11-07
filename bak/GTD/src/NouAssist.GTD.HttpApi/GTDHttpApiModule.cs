using Localization.Resources.AbpUi;
using NouAssist.GTD.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.GTD;

[DependsOn(
    typeof(GTDApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class GTDHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GTDHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<GTDResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
