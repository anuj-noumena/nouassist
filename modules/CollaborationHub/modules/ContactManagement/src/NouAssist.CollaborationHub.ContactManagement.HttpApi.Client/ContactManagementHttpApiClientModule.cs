using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(ContactManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ContactManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ContactManagementApplicationContractsModule).Assembly,
            ContactManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ContactManagementHttpApiClientModule>();
        });

    }
}
