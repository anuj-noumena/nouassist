using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(WorkflowEngineDomainModule),
    typeof(WorkflowEngineTestBaseModule)
)]
public class WorkflowEngineDomainTestModule : AbpModule
{

}
