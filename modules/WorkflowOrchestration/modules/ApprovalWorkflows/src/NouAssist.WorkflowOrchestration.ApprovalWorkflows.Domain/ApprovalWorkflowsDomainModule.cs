using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ApprovalWorkflowsDomainSharedModule)
)]
public class ApprovalWorkflowsDomainModule : AbpModule
{

}
