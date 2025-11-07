using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(WorkflowOrchestrationDomainSharedModule)
)]
public class WorkflowOrchestrationDomainModule : AbpModule
{

}
