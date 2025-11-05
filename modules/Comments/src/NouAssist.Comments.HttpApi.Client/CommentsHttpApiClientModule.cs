using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Comments;

[DependsOn(
    typeof(CommentsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CommentsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CommentsApplicationContractsModule).Assembly,
            CommentsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CommentsHttpApiClientModule>();
        });

    }
}
