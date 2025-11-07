using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GTDSystemDomainSharedModule)
)]
public class GTDSystemDomainModule : AbpModule
{

}
