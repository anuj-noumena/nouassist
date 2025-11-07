using Localization.Resources.AbpUi;
using NouAssist.Comments.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Comments;

[DependsOn(
    typeof(CommentsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CommentsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CommentsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CommentsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
