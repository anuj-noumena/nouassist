using Localization.Resources.AbpUi;
using NouAssist.LearningDevelopment.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class LearningDevelopmentHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(LearningDevelopmentHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<LearningDevelopmentResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
