using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(CollaborationHubDomainModule),
    typeof(CollaborationHubTestBaseModule)
)]
public class CollaborationHubDomainTestModule : AbpModule
{

}
