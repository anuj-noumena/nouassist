using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ResourcesDomainSharedModule)
)]
public class ResourcesDomainModule : AbpModule
{

}
