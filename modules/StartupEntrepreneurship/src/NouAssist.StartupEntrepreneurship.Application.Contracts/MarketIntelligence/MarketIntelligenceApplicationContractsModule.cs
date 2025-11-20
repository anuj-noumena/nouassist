using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence
{
    [DependsOn(
        typeof(MarketIntelligenceDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpValidationModule)
    )]
    public class MarketIntelligenceApplicationContractsModule : AbpModule
    {
    }
}
