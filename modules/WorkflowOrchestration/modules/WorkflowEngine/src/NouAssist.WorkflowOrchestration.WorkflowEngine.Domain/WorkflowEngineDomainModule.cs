using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(WorkflowEngineDomainSharedModule)
)]
public class WorkflowEngineDomainModule : AbpModule
{

}
