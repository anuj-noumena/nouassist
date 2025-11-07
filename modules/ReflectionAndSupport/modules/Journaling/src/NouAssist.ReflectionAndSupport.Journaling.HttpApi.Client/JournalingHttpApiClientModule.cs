using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(JournalingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class JournalingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(JournalingApplicationContractsModule).Assembly,
            JournalingRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<JournalingHttpApiClientModule>();
        });

    }
}
