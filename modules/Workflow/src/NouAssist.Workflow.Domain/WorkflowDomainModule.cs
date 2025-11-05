using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Workflow;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(WorkflowDomainSharedModule)
)]
public class WorkflowDomainModule : AbpModule
{

}
