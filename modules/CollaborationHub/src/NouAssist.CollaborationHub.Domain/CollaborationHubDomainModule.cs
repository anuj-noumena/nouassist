using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CollaborationHubDomainSharedModule)
)]
public class CollaborationHubDomainModule : AbpModule
{

}
