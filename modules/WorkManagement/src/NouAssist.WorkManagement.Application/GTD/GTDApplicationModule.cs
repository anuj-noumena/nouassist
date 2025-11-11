using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.GTD;

[DependsOn(
    typeof(GTDDomainModule),
    typeof(GTDApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class GTDApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<GTDApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<GTDApplicationModule>(validate: true);
        });
    }
}
