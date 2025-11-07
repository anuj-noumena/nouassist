using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(ReflectionAndSupportApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ReflectionAndSupportHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ReflectionAndSupportApplicationContractsModule).Assembly,
            ReflectionAndSupportRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ReflectionAndSupportHttpApiClientModule>();
        });

    }
}
