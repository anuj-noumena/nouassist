using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.GoalTracking;

[DependsOn(
    typeof(GoalTrackingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class GoalTrackingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(GoalTrackingApplicationContractsModule).Assembly,
            GoalTrackingRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GoalTrackingHttpApiClientModule>();
        });

    }
}
