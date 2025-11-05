using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Habits;

[DependsOn(
    typeof(HabitsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class HabitsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(HabitsApplicationContractsModule).Assembly,
            HabitsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<HabitsHttpApiClientModule>();
        });

    }
}
