using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(RequirementsManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class RequirementsManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(RequirementsManagementApplicationContractsModule).Assembly,
            RequirementsManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RequirementsManagementHttpApiClientModule>();
        });

    }
}
