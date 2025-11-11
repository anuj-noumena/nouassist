using Localization.Resources.AbpUi;
using NouAssist.PersonalDevelopment.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(PersonalDevelopmentApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class PersonalDevelopmentHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PersonalDevelopmentHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PersonalDevelopmentResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
