using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(IntelligenceStrategyDomainSharedModule)
)]
public class IntelligenceStrategyDomainModule : AbpModule
{

}
