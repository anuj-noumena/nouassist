using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(SharedKernelApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class SharedKernelHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(SharedKernelApplicationContractsModule).Assembly,
            SharedKernelRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SharedKernelHttpApiClientModule>();
        });

    }
}
