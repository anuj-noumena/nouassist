using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(WorkflowOrchestrationDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class WorkflowOrchestrationApplicationContractsModule : AbpModule
{

}
