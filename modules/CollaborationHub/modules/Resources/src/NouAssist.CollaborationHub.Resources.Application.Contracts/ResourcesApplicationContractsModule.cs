using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(ResourcesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ResourcesApplicationContractsModule : AbpModule
{

}
