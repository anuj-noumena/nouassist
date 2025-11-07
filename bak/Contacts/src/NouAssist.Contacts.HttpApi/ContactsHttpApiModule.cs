using Localization.Resources.AbpUi;
using NouAssist.Contacts.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NouAssist.Contacts;

[DependsOn(
    typeof(ContactsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ContactsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ContactsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ContactsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
