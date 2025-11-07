using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(GTDSystemDomainModule),
    typeof(GTDSystemTestBaseModule)
)]
public class GTDSystemDomainTestModule : AbpModule
{

}
