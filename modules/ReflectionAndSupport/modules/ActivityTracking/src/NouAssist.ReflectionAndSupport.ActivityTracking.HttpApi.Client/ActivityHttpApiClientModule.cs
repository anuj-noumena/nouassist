using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ActivityTrackingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ActivityTrackingApplicationContractsModule).Assembly,
            ActivityRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ActivityTrackingHttpApiClientModule>();
        });

    }
}
