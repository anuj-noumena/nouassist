using Localization.Resources.AbpUi;
using NouAssist.Users.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Users;

[DependsOn(
    typeof(UsersApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class UsersHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(UsersHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<UsersResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
