using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(CustomFieldsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CustomFieldsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CustomFieldsApplicationContractsModule).Assembly,
            CustomFieldsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CustomFieldsHttpApiClientModule>();
        });

    }
}
