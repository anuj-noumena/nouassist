using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Notification;

[DependsOn(
    typeof(NotificationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class NotificationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(NotificationApplicationContractsModule).Assembly,
            NotificationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<NotificationHttpApiClientModule>();
        });

    }
}
