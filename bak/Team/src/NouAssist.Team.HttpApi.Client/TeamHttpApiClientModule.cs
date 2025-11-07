using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Team;

[DependsOn(
    typeof(TeamApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TeamHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TeamApplicationContractsModule).Assembly,
            TeamRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TeamHttpApiClientModule>();
        });

    }
}
