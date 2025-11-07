using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(WorkflowOrchestrationApplicationModule),
    typeof(WorkflowOrchestrationDomainTestModule)
    )]
public class WorkflowOrchestrationApplicationTestModule : AbpModule
{

}
