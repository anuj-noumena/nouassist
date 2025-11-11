using Volo.Abp.Modularity;

namespace NouAssist.Collaboration;

[DependsOn(
    typeof(CollaborationApplicationModule),
    typeof(CollaborationDomainTestModule)
    )]
public class CollaborationApplicationTestModule : AbpModule
{

}
