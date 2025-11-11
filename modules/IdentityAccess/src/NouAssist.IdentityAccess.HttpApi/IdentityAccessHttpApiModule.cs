using Localization.Resources.AbpUi;
using NouAssist.IdentityAccess.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class IdentityAccessHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(IdentityAccessHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityAccessResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
