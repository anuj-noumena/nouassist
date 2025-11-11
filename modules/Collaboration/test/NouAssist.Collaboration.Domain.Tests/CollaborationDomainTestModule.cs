using Volo.Abp.Modularity;

namespace NouAssist.Collaboration;

[DependsOn(
    typeof(CollaborationDomainModule),
    typeof(CollaborationTestBaseModule)
)]
public class CollaborationDomainTestModule : AbpModule
{

}
