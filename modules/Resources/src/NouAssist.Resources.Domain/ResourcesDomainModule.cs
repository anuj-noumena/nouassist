using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Resources;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ResourcesDomainSharedModule)
)]
public class ResourcesDomainModule : AbpModule
{

}
