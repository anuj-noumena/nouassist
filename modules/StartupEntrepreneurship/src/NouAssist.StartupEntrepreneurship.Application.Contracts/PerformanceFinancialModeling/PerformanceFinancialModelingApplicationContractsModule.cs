using Volo.Abp.Authorization;
using Volo.Abp.Ddd.Application.Contracts;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling
{
    [DependsOn(
        typeof(PerformanceFinancialModelingDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class PerformanceFinancialModelingApplicationContractsModule : AbpModule
    {
    }
}
