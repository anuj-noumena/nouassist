using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.ProjectManagement;

[DependsOn(
    typeof(ProjectManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ProjectManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ProjectManagementApplicationContractsModule).Assembly,
            ProjectsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProjectManagementHttpApiClientModule>();
        });

    }
}
