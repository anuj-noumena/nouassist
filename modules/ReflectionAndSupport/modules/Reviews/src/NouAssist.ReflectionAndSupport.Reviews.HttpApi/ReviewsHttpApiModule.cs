using Localization.Resources.AbpUi;
using NouAssist.ReflectionAndSupport.Reviews.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.ReflectionAndSupport.Reviews;

[DependsOn(
    typeof(ReviewsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ReviewsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ReviewsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ReviewsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
