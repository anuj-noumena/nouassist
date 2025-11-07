using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(GTDSystemDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class GTDSystemApplicationContractsModule : AbpModule
{

}
