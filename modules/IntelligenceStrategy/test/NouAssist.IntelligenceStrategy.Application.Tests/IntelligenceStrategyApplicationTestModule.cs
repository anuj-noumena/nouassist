using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(IntelligenceStrategyApplicationModule),
    typeof(IntelligenceStrategyDomainTestModule)
    )]
public class IntelligenceStrategyApplicationTestModule : AbpModule
{

}
