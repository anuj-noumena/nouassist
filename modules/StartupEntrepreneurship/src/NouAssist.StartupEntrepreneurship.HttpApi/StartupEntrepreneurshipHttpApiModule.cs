using Localization.Resources.AbpUi;
using NouAssist.StartupEntrepreneurship.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(StartupEntrepreneurshipApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class StartupEntrepreneurshipHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(StartupEntrepreneurshipHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<StartupEntrepreneurshipResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
