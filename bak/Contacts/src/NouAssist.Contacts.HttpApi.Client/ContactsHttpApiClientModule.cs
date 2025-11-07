using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Contacts;

[DependsOn(
    typeof(ContactsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ContactsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ContactsApplicationContractsModule).Assembly,
            ContactsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ContactsHttpApiClientModule>();
        });

    }
}
