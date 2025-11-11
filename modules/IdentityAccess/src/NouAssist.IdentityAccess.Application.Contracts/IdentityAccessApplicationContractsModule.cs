using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class IdentityAccessApplicationContractsModule : AbpModule
{

}
