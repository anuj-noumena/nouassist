using Localization.Resources.AbpUi;
using NouAssist.ReflectionAndSupport.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(ReflectionAndSupportApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ReflectionAndSupportHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ReflectionAndSupportHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ReflectionAndSupportResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
