using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Collaboration;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CollaborationDomainSharedModule)
)]
public class CollaborationDomainModule : AbpModule
{

}
