using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(CollaborationHubDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CollaborationHubApplicationContractsModule : AbpModule
{

}
