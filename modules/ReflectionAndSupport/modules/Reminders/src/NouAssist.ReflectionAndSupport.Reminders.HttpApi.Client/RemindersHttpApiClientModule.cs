using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(RemindersApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class RemindersHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(RemindersApplicationContractsModule).Assembly,
            RemindersRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RemindersHttpApiClientModule>();
        });

    }
}
