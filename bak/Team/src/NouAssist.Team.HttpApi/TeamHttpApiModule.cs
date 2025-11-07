using Localization.Resources.AbpUi;
using NouAssist.Team.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Team;

[DependsOn(
    typeof(TeamApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class TeamHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(TeamHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TeamResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
