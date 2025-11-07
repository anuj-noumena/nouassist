using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

[DependsOn(
    typeof(ApprovalWorkflowsApplicationModule),
    typeof(ApprovalWorkflowsDomainTestModule)
    )]
public class ApprovalWorkflowsApplicationTestModule : AbpModule
{

}
