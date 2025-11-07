using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.TaskManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity.TaskManagement;

[DependsOn(
    typeof(TaskManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class TaskManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(TaskManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TaskManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
