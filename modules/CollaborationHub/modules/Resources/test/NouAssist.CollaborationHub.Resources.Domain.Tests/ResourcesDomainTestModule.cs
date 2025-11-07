using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(ResourcesDomainModule),
    typeof(ResourcesTestBaseModule)
)]
public class ResourcesDomainTestModule : AbpModule
{

}
