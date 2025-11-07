using Localization.Resources.AbpUi;
using NouAssist.Tasks.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(TasksApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class TasksHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(TasksHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TasksResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
