using Volo.Abp.Modularity;

namespace NouAssist.Resources;

[DependsOn(
    typeof(ResourcesDomainModule),
    typeof(ResourcesTestBaseModule)
)]
public class ResourcesDomainTestModule : AbpModule
{

}
