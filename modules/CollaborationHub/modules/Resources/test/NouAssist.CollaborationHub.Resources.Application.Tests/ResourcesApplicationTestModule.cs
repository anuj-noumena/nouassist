using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(ResourcesApplicationModule),
    typeof(ResourcesDomainTestModule)
    )]
public class ResourcesApplicationTestModule : AbpModule
{

}
