using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.UserProfiles;

[DependsOn(
    typeof(UserProfilesApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class UserProfilesHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(UserProfilesApplicationContractsModule).Assembly,
            UsersRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<UserProfilesHttpApiClientModule>();
        });

    }
}
