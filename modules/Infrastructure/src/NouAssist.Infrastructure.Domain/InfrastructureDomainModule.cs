using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(InfrastructureDomainSharedModule)
)]
public class InfrastructureDomainModule : AbpModule
{

}
