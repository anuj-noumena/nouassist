using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

[DependsOn(
    typeof(ApprovalWorkflowsDomainModule),
    typeof(ApprovalWorkflowsTestBaseModule)
)]
public class ApprovalWorkflowsDomainTestModule : AbpModule
{

}
