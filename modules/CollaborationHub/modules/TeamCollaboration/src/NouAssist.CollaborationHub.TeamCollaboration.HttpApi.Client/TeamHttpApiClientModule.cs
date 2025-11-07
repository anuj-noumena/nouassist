using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(TeamCollaborationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TeamCollaborationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TeamCollaborationApplicationContractsModule).Assembly,
            TeamRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TeamCollaborationHttpApiClientModule>();
        });

    }
}
