using Localization.Resources.AbpUi;
using NouAssist.FinancialManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class FinancialManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FinancialManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<FinancialManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
