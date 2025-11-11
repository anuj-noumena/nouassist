using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.Workflow;

[DependsOn(
    typeof(WorkflowDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class WorkflowApplicationContractsModule : AbpModule
{

}
