using Localization.Resources.AbpUi;
using NouAssist.Meetings.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(MeetingsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class MeetingsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(MeetingsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<MeetingsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
