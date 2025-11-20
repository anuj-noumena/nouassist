using Localization.Resources.AbpUi;
using NouAssist.IntelligenceStrategy.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(IntelligenceStrategyApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class IntelligenceStrategyHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(IntelligenceStrategyHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IntelligenceStrategyResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
