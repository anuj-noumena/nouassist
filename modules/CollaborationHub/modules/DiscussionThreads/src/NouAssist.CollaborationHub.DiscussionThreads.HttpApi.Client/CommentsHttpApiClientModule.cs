using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(DiscussionThreadsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class DiscussionThreadsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(DiscussionThreadsApplicationContractsModule).Assembly,
            CommentsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DiscussionThreadsHttpApiClientModule>();
        });

    }
}
