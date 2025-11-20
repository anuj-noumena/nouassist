using Volo.Abp.Modularity;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(IntegrationsApplicationModule),
    typeof(IntegrationsDomainTestModule)
    )]
public class IntegrationsApplicationTestModule : AbpModule
{

}
