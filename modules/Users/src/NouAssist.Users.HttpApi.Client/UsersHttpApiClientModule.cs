using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Users;

[DependsOn(
    typeof(UsersApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class UsersHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(UsersApplicationContractsModule).Assembly,
            UsersRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<UsersHttpApiClientModule>();
        });

    }
}
