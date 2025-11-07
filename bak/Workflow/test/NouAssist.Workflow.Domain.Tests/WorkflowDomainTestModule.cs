using Volo.Abp.Modularity;

namespace NouAssist.Workflow;

[DependsOn(
    typeof(WorkflowDomainModule),
    typeof(WorkflowTestBaseModule)
)]
public class WorkflowDomainTestModule : AbpModule
{

}
