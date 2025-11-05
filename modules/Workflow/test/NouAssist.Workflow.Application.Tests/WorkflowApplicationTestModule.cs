using Volo.Abp.Modularity;

namespace NouAssist.Workflow;

[DependsOn(
    typeof(WorkflowApplicationModule),
    typeof(WorkflowDomainTestModule)
    )]
public class WorkflowApplicationTestModule : AbpModule
{

}
