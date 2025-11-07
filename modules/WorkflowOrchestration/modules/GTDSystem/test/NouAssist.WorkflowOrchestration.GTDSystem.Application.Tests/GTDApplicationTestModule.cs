using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(GTDSystemApplicationModule),
    typeof(GTDSystemDomainTestModule)
    )]
public class GTDSystemApplicationTestModule : AbpModule
{

}
