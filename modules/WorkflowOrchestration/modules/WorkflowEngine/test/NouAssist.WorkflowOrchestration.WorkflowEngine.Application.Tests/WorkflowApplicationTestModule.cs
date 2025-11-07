using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(WorkflowEngineApplicationModule),
    typeof(WorkflowEngineDomainTestModule)
    )]
public class WorkflowEngineApplicationTestModule : AbpModule
{

}
