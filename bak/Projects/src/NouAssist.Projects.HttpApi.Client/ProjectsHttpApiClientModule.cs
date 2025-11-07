using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Projects;

[DependsOn(
    typeof(ProjectsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ProjectsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ProjectsApplicationContractsModule).Assembly,
            ProjectsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProjectsHttpApiClientModule>();
        });

    }
}
