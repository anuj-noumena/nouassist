using Localization.Resources.AbpUi;
using NouAssist.CollaborationHub.Resources.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(ResourcesApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ResourcesHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ResourcesHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ResourcesResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
