using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureDomainModule),
    typeof(InfrastructureTestBaseModule)
)]
public class InfrastructureDomainTestModule : AbpModule
{

}
