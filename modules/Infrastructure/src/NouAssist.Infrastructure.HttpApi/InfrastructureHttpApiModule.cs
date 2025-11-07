using Localization.Resources.AbpUi;
using NouAssist.Infrastructure.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class InfrastructureHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(InfrastructureHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<InfrastructureResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
