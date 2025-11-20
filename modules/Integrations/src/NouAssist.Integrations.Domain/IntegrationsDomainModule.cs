using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(IntegrationsDomainSharedModule)
)]
public class IntegrationsDomainModule : AbpModule
{

}
