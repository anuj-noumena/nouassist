using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(CollaborationHubApplicationModule),
    typeof(CollaborationHubDomainTestModule)
    )]
public class CollaborationHubApplicationTestModule : AbpModule
{

}
