using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(IntelligenceStrategyDomainModule),
    typeof(IntelligenceStrategyApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class IntelligenceStrategyApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IntelligenceStrategyApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<IntelligenceStrategyApplicationModule>(validate: true);
        });
    }
}
