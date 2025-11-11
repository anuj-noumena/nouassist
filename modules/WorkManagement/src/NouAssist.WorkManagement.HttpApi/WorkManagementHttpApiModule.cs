using Localization.Resources.AbpUi;
using NouAssist.WorkManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(WorkManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class WorkManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(WorkManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WorkManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
