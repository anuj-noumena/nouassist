using Volo.Abp.Modularity;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(IntegrationsDomainModule),
    typeof(IntegrationsTestBaseModule)
)]
public class IntegrationsDomainTestModule : AbpModule
{

}
