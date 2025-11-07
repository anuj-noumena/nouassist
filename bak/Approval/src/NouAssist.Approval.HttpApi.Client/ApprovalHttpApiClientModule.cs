using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Approval;

[DependsOn(
    typeof(ApprovalApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ApprovalHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ApprovalApplicationContractsModule).Assembly,
            ApprovalRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ApprovalHttpApiClientModule>();
        });

    }
}
