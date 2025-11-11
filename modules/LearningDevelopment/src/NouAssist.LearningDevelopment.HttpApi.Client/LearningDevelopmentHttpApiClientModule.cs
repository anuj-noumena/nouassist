using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class LearningDevelopmentHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(LearningDevelopmentApplicationContractsModule).Assembly,
            LearningDevelopmentRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<LearningDevelopmentHttpApiClientModule>();
        });

    }
}
