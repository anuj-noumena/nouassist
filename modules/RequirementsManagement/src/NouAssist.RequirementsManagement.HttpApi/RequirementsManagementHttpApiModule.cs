using Localization.Resources.AbpUi;
using NouAssist.RequirementsManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(RequirementsManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class RequirementsManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(RequirementsManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<RequirementsManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
