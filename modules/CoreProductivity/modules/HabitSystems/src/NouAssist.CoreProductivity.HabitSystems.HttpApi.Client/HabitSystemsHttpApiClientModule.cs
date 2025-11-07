using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(HabitSystemsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class HabitSystemsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(HabitSystemsApplicationContractsModule).Assembly,
            HabitSystemsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<HabitSystemsHttpApiClientModule>();
        });

    }
}
