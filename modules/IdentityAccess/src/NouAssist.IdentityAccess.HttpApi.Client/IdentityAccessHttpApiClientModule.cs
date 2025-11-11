using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IdentityAccessHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IdentityAccessApplicationContractsModule).Assembly,
            IdentityAccessRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityAccessHttpApiClientModule>();
        });

    }
}
