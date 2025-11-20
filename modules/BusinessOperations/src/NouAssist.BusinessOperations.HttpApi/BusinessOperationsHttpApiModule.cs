using Localization.Resources.AbpUi;
using NouAssist.BusinessOperations.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(BusinessOperationsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class BusinessOperationsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BusinessOperationsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<BusinessOperationsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
