using Localization.Resources.AbpUi;
using NouAssist.AssetManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AssetManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AssetManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AssetManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
