using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(IntelligenceStrategyDomainModule),
    typeof(IntelligenceStrategyTestBaseModule)
)]
public class IntelligenceStrategyDomainTestModule : AbpModule
{

}
