using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(MeetingsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class MeetingsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(MeetingsApplicationContractsModule).Assembly,
            MeetingsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MeetingsHttpApiClientModule>();
        });

    }
}
