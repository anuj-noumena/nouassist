using Localization.Resources.AbpUi;
using NouAssist.CustomFields.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CustomFields;

[DependsOn(
    typeof(CustomFieldsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CustomFieldsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CustomFieldsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CustomFieldsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
