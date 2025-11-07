using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.Reviews;

[DependsOn(
    typeof(ReviewsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ReviewsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ReviewsApplicationContractsModule).Assembly,
            ReviewsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ReviewsHttpApiClientModule>();
        });

    }
}
