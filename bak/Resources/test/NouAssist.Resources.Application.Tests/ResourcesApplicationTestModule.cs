using Volo.Abp.Modularity;

namespace NouAssist.Resources;

[DependsOn(
    typeof(ResourcesApplicationModule),
    typeof(ResourcesDomainTestModule)
    )]
public class ResourcesApplicationTestModule : AbpModule
{

}
