using Localization.Resources.AbpUi;
using NouAssist.CoreProductivity.ProjectManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CoreProductivity.ProjectManagement;

[DependsOn(
    typeof(ProjectManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ProjectManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ProjectManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
