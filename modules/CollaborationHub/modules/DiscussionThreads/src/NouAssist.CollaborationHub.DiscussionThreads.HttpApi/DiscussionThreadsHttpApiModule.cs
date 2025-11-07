using Localization.Resources.AbpUi;
using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(DiscussionThreadsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class DiscussionThreadsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(DiscussionThreadsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DiscussionThreadsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
