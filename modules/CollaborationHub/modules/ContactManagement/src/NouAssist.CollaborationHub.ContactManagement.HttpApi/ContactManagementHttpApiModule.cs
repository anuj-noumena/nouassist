using Localization.Resources.AbpUi;
using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(ContactManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ContactManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ContactManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ContactManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
