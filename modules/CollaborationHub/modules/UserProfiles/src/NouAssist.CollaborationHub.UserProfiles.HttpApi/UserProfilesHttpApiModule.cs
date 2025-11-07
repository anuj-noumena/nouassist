using Localization.Resources.AbpUi;
using NouAssist.CollaborationHub.UserProfiles.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(UserProfilesApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class UserProfilesHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(UserProfilesHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<UserProfilesResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
