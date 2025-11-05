using Localization.Resources.AbpUi;
using NouAssist.SharedKernel.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(SharedKernelApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class SharedKernelHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(SharedKernelHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SharedKernelResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
