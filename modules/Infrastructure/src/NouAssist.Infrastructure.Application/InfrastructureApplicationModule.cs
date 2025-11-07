using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureDomainModule),
    typeof(InfrastructureApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class InfrastructureApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<InfrastructureApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<InfrastructureApplicationModule>(validate: true);
        });
    }
}
