using Localization.Resources.AbpUi;
using NouAssist.CollaborationHub.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(CollaborationHubApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CollaborationHubHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CollaborationHubHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CollaborationHubResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
