using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(BusinessOperationsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class BusinessOperationsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(BusinessOperationsApplicationContractsModule).Assembly,
            BusinessOperationsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BusinessOperationsHttpApiClientModule>();
        });

    }
}
