using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(WorkflowOrchestrationDomainModule),
    typeof(WorkflowOrchestrationTestBaseModule)
)]
public class WorkflowOrchestrationDomainTestModule : AbpModule
{

}
